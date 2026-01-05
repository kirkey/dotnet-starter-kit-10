using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Payees.CreatePayee;

/// <summary>
/// Command to create a new payee entity.
/// </summary>
/// <param name="Name">Payee name (required, unique per tenant)</param>
/// <param name="Description">Optional payee description</param>
public record CreatePayeeCommand(string Name, string? Description) : ICommand<Guid>;