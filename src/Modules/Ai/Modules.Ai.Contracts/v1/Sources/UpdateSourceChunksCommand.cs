using Mediator;

namespace FSH.Modules.Ai.Contracts.v1.Sources;

public sealed record UpdateSourceChunksCommand(Guid SourceId) : ICommand<Guid>;
