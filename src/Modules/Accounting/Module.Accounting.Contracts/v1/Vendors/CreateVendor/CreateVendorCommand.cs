using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Vendors.CreateVendor;

/// <summary>
/// Command to create a new vendor entity.
/// </summary>
/// <param name="Name">Vendor business name (required, unique per tenant)</param>
/// <param name="Description">Optional vendor description, notes, or additional information</param>
public record CreateVendorCommand(string Name, string? Description) : ICommand<Guid>;