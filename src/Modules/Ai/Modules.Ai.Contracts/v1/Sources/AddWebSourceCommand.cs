using Mediator;

namespace FSH.Modules.Ai.Contracts.v1.Sources;

public sealed record AddWebSourceCommand(string Url, string? Name) : ICommand<Guid>;
