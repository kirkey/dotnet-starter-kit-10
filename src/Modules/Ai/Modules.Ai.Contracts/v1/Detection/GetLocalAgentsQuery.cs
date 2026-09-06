using FSH.Modules.Ai.Contracts.Dtos;
using Mediator;

namespace FSH.Modules.Ai.Contracts.v1.Detection;

public sealed record GetLocalAgentsQuery : IQuery<IReadOnlyList<DetectedAgentDto>>;
