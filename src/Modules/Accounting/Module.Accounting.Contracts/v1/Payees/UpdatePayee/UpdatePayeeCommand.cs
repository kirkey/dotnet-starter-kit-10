using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Payees.UpdatePayee;

/// <summary>
/// Command to update an existing payee's metadata.
/// </summary>
/// <param name="Id">Payee ID to update (must exist)</param>
/// <param name="Name">Updated payee name</param>
/// <param name="Description">Updated payee description</param>
public record UpdatePayeeCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;