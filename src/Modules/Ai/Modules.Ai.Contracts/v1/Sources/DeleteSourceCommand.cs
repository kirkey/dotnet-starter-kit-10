using Mediator;

namespace FSH.Modules.Ai.Contracts.v1.Sources;

public sealed record DeleteSourceCommand(Guid SourceId) : ICommand<Guid>;
