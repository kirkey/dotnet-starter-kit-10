using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Customers.DeleteCustomer;

/// <summary>
/// Command to delete an existing customer from the system.
/// </summary>
/// <param name="Id">Customer ID to delete (must exist and have no invoices)</param>
public record DeleteCustomerCommand(Guid Id) : ICommand;