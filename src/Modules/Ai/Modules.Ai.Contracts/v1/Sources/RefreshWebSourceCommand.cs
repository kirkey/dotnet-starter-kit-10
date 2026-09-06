using Mediator;

namespace FSH.Modules.Ai.Contracts.v1.Sources;

public sealed record RefreshWebSourceCommand(Guid SourceId) : ICommand<Guid>;
