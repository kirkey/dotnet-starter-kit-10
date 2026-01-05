using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Vendors.DeleteVendor;

/// <summary>
/// Command to delete an existing vendor from the system.
/// </summary>
/// <param name="Id">Vendor ID to delete (must exist)</param>
public record DeleteVendorCommand(Guid Id) : ICommand;