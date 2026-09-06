using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Domain;

namespace FSH.Modules.Ai.Features.v1.Providers;

internal static class AiProviderMapper
{
    public static AiProviderDto ToDto(AiProvider provider, bool hasApiKey) =>
        new(
            provider.Id,
            provider.Name,
            provider.ProviderType,
            provider.BaseUrl,
            provider.ChatModel,
            provider.EmbeddingModel,
            provider.EmbeddingDimensions,
            provider.IsDefaultChat,
            provider.IsDefaultEmbedding,
            provider.Revision,
            hasApiKey,
            provider.Models
                .Select(m => new AiProviderModelDto(m.ModelId, m.DisplayName, m.SupportsChat, m.SupportsEmbeddings))
                .ToList());
}
