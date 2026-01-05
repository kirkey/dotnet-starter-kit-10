using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Vendors.UpdateVendor;

/// <summary>
/// Command to update an existing vendor's metadata.
/// </summary>
/// <param name="Id">Vendor ID to update (must exist)</param>
/// <param name="Name">Updated vendor name</param>
/// <param name="Description">Updated vendor description or null to clear</param>
public record UpdateVendorCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;