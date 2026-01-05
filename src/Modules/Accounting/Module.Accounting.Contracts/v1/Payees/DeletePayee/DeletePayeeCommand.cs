using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Payees.DeletePayee;

/// <summary>
/// Command to delete an existing payee.
/// </summary>
/// <param name="Id">Payee ID to delete</param>
public record DeletePayeeCommand(Guid Id) : ICommand;