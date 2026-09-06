using Mediator;

namespace FSH.Modules.Ai.Contracts.v1.Providers;

public sealed record SetProviderDefaultCommand(Guid ProviderId, bool Chat, bool Embedding) : ICommand<Guid>;
