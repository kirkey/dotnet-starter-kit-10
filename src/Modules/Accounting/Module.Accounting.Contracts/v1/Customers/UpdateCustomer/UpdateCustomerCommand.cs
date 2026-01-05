using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Customers.UpdateCustomer;

/// <summary>
/// Command to update an existing customer's metadata.
/// </summary>
/// <param name="Id">Customer ID to update (must exist)</param>
/// <param name="Name">Updated customer name</param>
/// <param name="Description">Updated customer description or null to clear</param>
public record UpdateCustomerCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;