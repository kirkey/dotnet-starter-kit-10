using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Runs;
using FSH.Modules.Ai.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Runs;

internal static class AiAgentRunMapper
{
    public static AiAgentRunDto ToDto(Domain.AiAgentRun run) =>
        new(
            run.Id, run.AgentId, run.Trigger, run.Status, run.Input, run.Output,
            run.Error, run.RetryCount, run.StartedOnUtc, run.FinishedOnUtc);
}
