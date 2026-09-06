using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Detection;
using FSH.Modules.Ai.Services;
using Mediator;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FSH.Modules.Ai.Features.v1.Detection.GetLocalAgents;

public sealed class GetLocalAgentsQueryHandler(
    ILocalAgentDetector detector,
    IOptions<AiOptions> options,
    ILogger<GetLocalAgentsQueryHandler> logger)
    : IQueryHandler<GetLocalAgentsQuery, IReadOnlyList<DetectedAgentDto>>
{
    public ValueTask<IReadOnlyList<DetectedAgentDto>> Handle(GetLocalAgentsQuery query, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(query);

        // Detection probes the server machine: only where the deployment permits (local/dev).
        // Elsewhere (and when nothing is installed) the picker degrades to models-only, no error.
        if (!options.Value.DetectionEnabled)
        {
            return ValueTask.FromResult<IReadOnlyList<DetectedAgentDto>>([]);
        }

        IReadOnlyList<DetectedAgentDto> found;
        try
        {
            found = detector.Detect()
                .Select(d => new DetectedAgentDto(d.Family, d.DisplayName, d.DetectedVersion, d.HasCredentials))
                .ToList();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            logger.LogWarning(ex, "[Ai] local agent detection failed; degrading to models-only");
            found = [];
        }

        return ValueTask.FromResult(found);
    }
}
