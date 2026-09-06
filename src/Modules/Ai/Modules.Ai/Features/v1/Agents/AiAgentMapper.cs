using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Domain;

namespace FSH.Modules.Ai.Features.v1.Agents;

internal static class AiAgentMapper
{
    public static AiAgentDto ToDto(AiAgent agent) =>
        new(
            agent.Id,
            agent.DepartmentId,
            agent.Name,
            agent.Instructions,
            agent.Skills(),
            agent.RuntimeBinding,
            agent.Model,
            agent.Variant,
            agent.AccessMode,
            agent.AccessUserIds(),
            agent.IsArchived);
}
