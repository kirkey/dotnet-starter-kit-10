using System.Net;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FSH.Modules.Ai.Services;

public interface IAiProviderSelector
{
    Task<ChatTarget> ResolveChatTargetAsync(string? model, Guid? agentId, AiVariant? variant, CancellationToken ct = default);
    Task<IChatClient> SelectChatClientAsync(string? model, Guid? agentId, CancellationToken ct = default);
    Task<IEmbeddingClient> SelectEmbeddingClientAsync(CancellationToken ct = default);
}

/// <summary>Model + variant snapshot a session or run executes against.</summary>
public sealed record ChatTarget(string Model, AiVariant Variant);

/// <summary>
/// Resolves which provider serves a capability: explicit model → agent's model → tenant chat/
/// embedding default → unambiguous single candidate → loud 503 naming the missing piece.
/// Archived agents and unknown models fail at selection time, never mid-run.
/// </summary>
public sealed class AiProviderSelector(
    AiDbContext db,
    IAiSecretProtector secrets,
    IHttpClientFactory httpFactory,
    ILogger<AiProviderSelector> logger)
    : IAiProviderSelector
{
    public async Task<ChatTarget> ResolveChatTargetAsync(
        string? model, Guid? agentId, AiVariant? variant, CancellationToken ct = default)
    {
        if (agentId.HasValue)
        {
            var agent = await db.Agents
                .FirstOrDefaultAsync(a => a.Id == agentId.Value, ct)
                .ConfigureAwait(false)
                ?? throw new NotFoundException($"Agent {agentId} was not found.");
            if (agent.IsArchived)
            {
                throw new CustomException(
                    $"Agent '{agent.Name}' is archived and takes no runs.",
                    errors: null,
                    HttpStatusCode.Conflict);
            }

            return new ChatTarget(agent.Model, agent.Variant);
        }

        if (!string.IsNullOrWhiteSpace(model))
        {
            var serves = await db.Providers
                .AnyAsync(p => p.ChatModel == model || p.Models.Any(m => m.ModelId == model && m.SupportsChat), ct)
                .ConfigureAwait(false);
            if (!serves)
            {
                if (IsLocalModel(model))
                {
                    throw Misconfigured($"Model '{model}' is served by the built-in local provider, which has been removed. Re-add it or pick a configured model.");
                }

                throw Misconfigured($"Model '{model}' is not served by any configured provider. Register it or pick a configured model.");
            }

            return new ChatTarget(model, variant ?? AiVariant.Default);
        }

        var defaultProvider = await db.Providers
            .Include(p => p.Models)
            .Where(p => p.IsDefaultChat)
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        if (defaultProvider is not null)
        {
            var defaultModel = defaultProvider.ChatModel
                ?? defaultProvider.Models.FirstOrDefault(m => m.SupportsChat)?.ModelId
                ?? (defaultProvider.ProviderType == AiProviderType.Local ? LocalChatClient.LocalModelName : null);
            if (defaultModel is not null)
            {
                return new ChatTarget(defaultModel, variant ?? AiVariant.Default);
            }
        }

        var single = await SingleCandidateAsync(supportsEmbeddings: false, ct).ConfigureAwait(false);
        if (single is not null)
        {
            var singleModel = single.ChatModel
                ?? single.Models.FirstOrDefault(m => m.SupportsChat)?.ModelId
                ?? (single.ProviderType == AiProviderType.Local ? LocalChatClient.LocalModelName : null);
            if (singleModel is not null)
            {
                return new ChatTarget(singleModel, variant ?? AiVariant.Default);
            }
        }

        throw Misconfigured("No chat provider is configured. Register a provider and mark it as the chat default.");
    }

    public async Task<IChatClient> SelectChatClientAsync(string? model, Guid? agentId, CancellationToken ct = default)
    {
        var target = await ResolveChatTargetAsync(model, agentId, null, ct).ConfigureAwait(false);
        var provider = await FindChatProviderAsync(target.Model, ct).ConfigureAwait(false);
        if (provider.ProviderType == AiProviderType.Local)
        {
            return new LocalChatClient();
        }

        return new OpenAiChatClient(
            httpFactory,
            provider.BaseUrl ?? throw Misconfigured($"Provider '{provider.Name}' has no endpoint URL."),
            target.Model,
            await ResolveApiKeyAsync(provider.Id, provider.Name, ct).ConfigureAwait(false));
    }

    public async Task<IEmbeddingClient> SelectEmbeddingClientAsync(CancellationToken ct = default)
    {
        var provider = await db.Providers
                .Include(p => p.Models)
                .Where(p => p.IsDefaultEmbedding)
                .FirstOrDefaultAsync(ct)
                .ConfigureAwait(false)
            ?? await SingleCandidateAsync(supportsEmbeddings: true, ct).ConfigureAwait(false)
            ?? throw Misconfigured("No embedding provider is configured. Register a provider and mark it as the embedding default.");

        if (provider.ProviderType == AiProviderType.Local)
        {
            return new LocalEmbeddingClient();
        }

        return new OpenAiEmbeddingClient(
            httpFactory,
            provider.BaseUrl ?? throw Misconfigured($"Provider '{provider.Name}' has no endpoint URL."),
            provider.EmbeddingModel ?? throw Misconfigured($"Provider '{provider.Name}' has no embedding model."),
            await ResolveApiKeyAsync(provider.Id, provider.Name, ct).ConfigureAwait(false));
    }

    private async Task<Domain.AiProvider> FindChatProviderAsync(string? model, CancellationToken ct)
    {
        var query = db.Providers.Include(p => p.Models).AsQueryable();
        if (!string.IsNullOrWhiteSpace(model))
        {
            var match = await query
                .Where(p => p.ChatModel == model || p.Models.Any(m => m.ModelId == model && m.SupportsChat))
                .FirstOrDefaultAsync(ct)
                .ConfigureAwait(false);
            if (match is not null)
            {
                return match;
            }

            if (IsLocalModel(model))
            {
                throw Misconfigured($"Model '{model}' is served by the built-in local provider, which has been removed. Re-add it or pick a configured model.");
            }

            throw Misconfigured($"Model '{model}' is not served by any configured provider. Register it or pick a configured model.");
        }

        return await query.Where(p => p.IsDefaultChat).FirstOrDefaultAsync(ct).ConfigureAwait(false)
            ?? await SingleCandidateAsync(supportsEmbeddings: false, ct).ConfigureAwait(false)
            ?? throw Misconfigured("No chat provider is configured. Register a provider and mark it as the chat default.");
    }

    private async Task<Domain.AiProvider?> SingleCandidateAsync(bool supportsEmbeddings, CancellationToken ct)
    {
        var candidates = await db.Providers.Include(p => p.Models).ToListAsync(ct).ConfigureAwait(false);
        var capable = candidates.Where(p =>
            p.ProviderType == AiProviderType.Local
            || (supportsEmbeddings
                ? p.EmbeddingModel is not null || p.Models.Any(m => m.SupportsEmbeddings)
                : p.ChatModel is not null || p.Models.Any(m => m.SupportsChat))).ToList();
        if (capable.Count > 1)
        {
            var capability = supportsEmbeddings ? "embedding" : "chat";
            throw Misconfigured($"Multiple {capability} providers are configured but none is marked default. Mark one as the {capability} default.");
        }

        return capable.SingleOrDefault();
    }

    private async Task<string> ResolveApiKeyAsync(Guid providerId, string providerName, CancellationToken ct)
    {
        // Secrets resolve at selection time only; the only sink is the outbound Authorization header.
        var protectedValue = await db.ProviderSecrets
            .Where(s => s.ProviderId == providerId && s.KeyName == "apiKey")
            .Select(s => s.ProtectedValue)
            .FirstOrDefaultAsync(ct)
            .ConfigureAwait(false);
        var key = secrets.Unprotect(protectedValue);

        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("Resolved provider {Provider} (key configured: {HasKey})", providerName, key is not null);
        }

        return key ?? string.Empty;
    }

    private static bool IsLocalModel(string model) =>
        string.Equals(model, LocalChatClient.LocalModelName, StringComparison.OrdinalIgnoreCase)
        || string.Equals(model, LocalEmbeddingClient.LocalModelName, StringComparison.OrdinalIgnoreCase);

    private static CustomException Misconfigured(string message) =>
        new(message, errors: null, HttpStatusCode.ServiceUnavailable);
}
