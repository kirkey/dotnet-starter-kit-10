using FSH.Module.Accounting.Contracts.v1.Vendors;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Vendors.GetVendor;

/// <summary>
/// Query to retrieve a single vendor by ID.
/// </summary>
/// <param name="Id">Vendor ID (Guid) to retrieve</param>
public record GetVendorQuery(Guid Id) : IQuery<VendorDto>;