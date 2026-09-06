using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Mailing;
using FSH.Framework.Mailing.Services;
using FSH.Framework.Shared.Multitenancy;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Domain;
using FSH.Modules.Ai.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FSH.Modules.Ai.Jobs;

/// <summary>
/// Executes one agent run: manual prompt runs complete against the agent's model; scheduled
/// web-watch runs fetch + hash + email-on-change; scheduled prompt runs complete + email when
/// recipients are set. Retries are explicit and visible (RetryCount on the row, max 3 attempts);
/// terminal failures record their cause on the row instead of failing silently.
/// </summary>
public sealed class AgentRunJob(
    IWebPageFetcher fetcher,
    IHtmlToMarkdownConverter converter,
    IMailService mail,
    IServiceScopeFactory scopeFactory,
    ILogger<AgentRunJob> logger)
{
    private const int MaxAttempts = 3;

    public async Task ExecuteAsync(string tenantId, Guid runId, CancellationToken ct)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantId);

        using var scope = scopeFactory.CreateScope();
        var tenant = await scope.ServiceProvider
            .GetRequiredService<IMultiTenantStore<AppTenantInfo>>()
            .GetAsync(tenantId)
            .ConfigureAwait(false);
        if (tenant is null)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                logger.LogWarning("[Ai] run skipped: tenant '{TenantId}' not found", tenantId);
            }

            return;
        }

        scope.ServiceProvider.GetRequiredService<IMultiTenantContextSetter>().MultiTenantContext =
            new MultiTenantContext<AppTenantInfo>(tenant);

        var db = scope.ServiceProvider.GetRequiredService<AiDbContext>();
        var selector = scope.ServiceProvider.GetRequiredService<IAiProviderSelector>();

        var run = await db.AgentRuns
            .FirstOrDefaultAsync(r => r.Id == runId, ct)
            .ConfigureAwait(false);
        if (run is null)
        {
            if (logger.IsEnabled(LogLevel.Warning))
            {
                logger.LogWarning("[Ai] run skipped: run {RunId} not found", runId);
            }

            return;
        }

        // At-least-once guard: a retried/crashed execution never re-runs terminal rows.
        if (run.Status is AgentRunStatus.Completed or AgentRunStatus.Failed or AgentRunStatus.Cancelled)
        {
            return;
        }

        var agent = await db.Agents
            .FirstOrDefaultAsync(a => a.Id == run.AgentId, ct)
            .ConfigureAwait(false);
        if (agent is null)
        {
            await FailAsync(db, run, "The agent no longer exists.", ct).ConfigureAwait(false);
            return;
        }

        if (agent.IsArchived)
        {
            await FailAsync(db, run, $"Agent '{agent.Name}' is archived and takes no runs.", ct).ConfigureAwait(false);
            return;
        }

        AiAgentSchedule? schedule = null;
        if (run.ScheduleId.HasValue)
        {
            schedule = await db.AgentSchedules
                .FirstOrDefaultAsync(s => s.Id == run.ScheduleId.Value, ct)
                .ConfigureAwait(false);
        }

        run.MarkRunning();
        await db.SaveChangesAsync(ct).ConfigureAwait(false);

        try
        {
            var outcome = await ExecuteWithRetryAsync(db, selector, run, agent, schedule, ct).ConfigureAwait(false);
            run.MarkCompleted(outcome.Output);
            schedule?.RecordRun(outcome.ContentHash);
            await db.SaveChangesAsync(ct).ConfigureAwait(false);

            if (outcome.Recipients.Count > 0 && (outcome.ShouldEmail || schedule is null))
            {
                await SendEmailAsync(outcome, agent, run, ct).ConfigureAwait(false);
            }

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("[Ai] run {RunId} completed (attempts used: {Retries})", runId, run.RetryCount + 1);
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            await FailAsync(db, run, TrimReason(ex.Message), ct).ConfigureAwait(false);
        }
    }

    private async Task<RunOutcome> ExecuteWithRetryAsync(
        AiDbContext db, IAiProviderSelector selector, AiAgentRun run, AiAgent agent, AiAgentSchedule? schedule, CancellationToken ct)
    {
        var attempt = 0;
        while (true)
        {
            try
            {
                return await ExecuteOnceAsync(selector, run, agent, schedule, ct).ConfigureAwait(false);
            }
            catch (Exception ex) when (IsTransient(ex) && attempt + 1 < MaxAttempts)
            {
                attempt++;
                run.RecordRetry();
                await db.SaveChangesAsync(ct).ConfigureAwait(false);

                if (logger.IsEnabled(LogLevel.Warning))
                {
                    logger.LogWarning(ex, "[Ai] run {RunId} attempt {Attempt} failed transiently; retrying", run.Id, attempt);
                }

                await Task.Delay(TimeSpan.FromSeconds(5 * attempt), ct).ConfigureAwait(false);
            }
        }
    }

    private async Task<RunOutcome> ExecuteOnceAsync(
        IAiProviderSelector selector, AiAgentRun run, AiAgent agent, AiAgentSchedule? schedule, CancellationToken ct)
    {
        if (schedule is null)
        {
            var chat = await selector.SelectChatClientAsync(agent.Model, agent.Id, ct).ConfigureAwait(false);
            var answer = await chat.CompleteAsync(
                [new ChatTurn("system", agent.Instructions), new ChatTurn("user", run.Input)],
                [],
                agent.Variant.ToString(),
                allowUngrounded: true,
                ct).ConfigureAwait(false);
            return new RunOutcome(answer.Text, null, [], ShouldEmail: false);
        }

        var config = ParseConfig(schedule);
        if (schedule.TaskType == ScheduleTaskType.WebWatch)
        {
            var url = config.Url ?? throw new InvalidOperationException($"Schedule '{schedule.Name}' has no URL configured.");
            var page = await fetcher.FetchAsync(url, ct).ConfigureAwait(false);
            var (_, markdown) = converter.Convert(page.Html, schedule.Name);
            if (string.IsNullOrWhiteSpace(markdown))
            {
                throw new InvalidOperationException($"'{url}' yielded no readable content.");
            }

            var hash = Sha256(markdown);
            if (hash == schedule.LastContentHash)
            {
                return new RunOutcome($"No changes at {url}.", hash, [], ShouldEmail: false);
            }

            var summary = markdown.Length <= 2000 ? markdown : markdown[..2000] + "…";
            return new RunOutcome($"Change detected at {url}:\n\n{summary}", hash, config.Recipients, ShouldEmail: true);
        }

        return await ExecutePromptRunAsync(selector, run, agent, config, ct).ConfigureAwait(false);
    }

    private static async Task<RunOutcome> ExecutePromptRunAsync(
        IAiProviderSelector selector, AiAgentRun run, AiAgent agent, ScheduleConfig config, CancellationToken ct)
    {
        var chat = await selector.SelectChatClientAsync(agent.Model, agent.Id, ct).ConfigureAwait(false);
        var prompt = string.IsNullOrWhiteSpace(config.Prompt) ? run.Input : config.Prompt;
        var answer = await chat.CompleteAsync(
            [new ChatTurn("system", agent.Instructions), new ChatTurn("user", prompt)],
            [],
            agent.Variant.ToString(),
            allowUngrounded: true,
            ct).ConfigureAwait(false);
        return new RunOutcome(answer.Text, null, config.Recipients, ShouldEmail: true);
    }

    private async Task SendEmailAsync(RunOutcome outcome, AiAgent agent, AiAgentRun run, CancellationToken ct)
    {
        try
        {
            await mail.SendAsync(new MailRequest(
                to: new Collection<string>(outcome.Recipients.ToList()),
                subject: $"[AI agent {agent.Name}] run completed",
                body: outcome.Output), ct).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "[Ai] run {RunId} email delivery failed", run.Id);
        }
    }

    private async Task FailAsync(AiDbContext db, AiAgentRun run, string reason, CancellationToken ct)
    {
        run.MarkFailed(reason);
        await db.SaveChangesAsync(ct).ConfigureAwait(false);

        if (logger.IsEnabled(LogLevel.Warning))
        {
            logger.LogWarning("[Ai] run {RunId} failed: {Reason}", run.Id, reason);
        }
    }

    private static bool IsTransient(Exception ex) =>
        ex is HttpRequestException || ex is TaskCanceledException || ex is TimeoutException;

    private static ScheduleConfig ParseConfig(AiAgentSchedule schedule)
    {
        try
        {
            using var doc = JsonDocument.Parse(schedule.TaskConfigJson);
            var root = doc.RootElement;
            return new ScheduleConfig(
                root.TryGetProperty("url", out var url) ? url.GetString() : null,
                root.TryGetProperty("prompt", out var prompt) ? prompt.GetString() : null,
                root.TryGetProperty("recipients", out var recipients)
                    ? recipients.EnumerateArray().Select(r => r.GetString() ?? string.Empty)
                        .Where(s => s.Length > 0).ToList()
                    : []);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Schedule '{schedule.Name}' has invalid task configuration.", ex);
        }
    }

    private static string Sha256(string text) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(text)));

    private static string TrimReason(string message) =>
        message.Length <= 500 ? message : message[..500];

    private sealed record ScheduleConfig(string? Url, string? Prompt, IReadOnlyList<string> Recipients);

    private sealed record RunOutcome(string Output, string? ContentHash, IReadOnlyList<string> Recipients, bool ShouldEmail);
}
