using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Runtimes;
using FSH.Modules.Ai.Services;
using Mediator;
using Microsoft.Extensions.Logging;

namespace FSH.Modules.Ai.Features.v1.Runtimes.RefreshRuntimeCatalog;

public sealed class RefreshRuntimeCatalogCommandHandler(
    ILocalAgentDetector detector,
    RuntimeCatalogService catalog,
    ILogger<RefreshRuntimeCatalogCommandHandler> logger)
    : ICommandHandler<RefreshRuntimeCatalogCommand, IReadOnlyList<AiRuntimeDto>>
{
    public async ValueTask<IReadOnlyList<AiRuntimeDto>> Handle(RefreshRuntimeCatalogCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        IReadOnlyList<DetectedAgent> detected;
        try
        {
            detected = detector.Detect();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            logger.LogWarning(ex, "[Ai] runtime catalog refresh skipped failing detection");
            detected = [];
        }

        var rows = await catalog.ReconcileAsync(detected, cancellationToken).ConfigureAwait(false);
        return rows.Select(ToDto).ToList();
    }

    internal static AiRuntimeDto ToDto(Domain.AiRuntime runtime) =>
        new(
            runtime.Id, runtime.Family, runtime.DisplayName, runtime.DetectedVersion,
            runtime.IsOnline, runtime.LastSeenOnUtc);
}
