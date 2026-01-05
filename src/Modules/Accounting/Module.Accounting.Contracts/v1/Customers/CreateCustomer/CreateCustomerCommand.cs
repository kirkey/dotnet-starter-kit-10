using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Customers.CreateCustomer;

/// <summary>
/// Command to create a new customer entity.
/// </summary>
/// <param name="Name">Customer business name or individual name (required, unique per tenant)</param>
/// <param name="Description">Optional customer description, notes, or additional information</param>
public record CreateCustomerCommand(string Name, string? Description) : ICommand<Guid>;