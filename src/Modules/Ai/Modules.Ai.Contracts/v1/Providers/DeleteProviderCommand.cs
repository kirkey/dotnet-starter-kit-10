using Mediator;

namespace FSH.Modules.Ai.Contracts.v1.Providers;

public sealed record DeleteProviderCommand(Guid Id) : ICommand<Guid>;
