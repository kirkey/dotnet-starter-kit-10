using Mediator;

namespace FSH.Modules.Ai.Contracts.v1.Providers;

public sealed record SetProviderSecretCommand(Guid ProviderId, string? KeyName, string Value) : ICommand<Guid>;
