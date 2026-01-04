using Mediator;

namespace FSH.Module.Identity.Contracts.v1.Users.ConfirmEmail;

public sealed record ConfirmEmailCommand(string UserId, string Code, string Tenant) : ICommand<string>;

