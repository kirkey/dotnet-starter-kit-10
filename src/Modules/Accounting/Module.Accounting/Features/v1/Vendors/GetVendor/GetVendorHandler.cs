using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Vendors;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Vendors.GetVendor;

public record GetVendorQuery(Guid Id) : IQuery<VendorDto>;

public class GetVendorHandler(AccountingDbContext context) : IQueryHandler<GetVendorQuery, VendorDto>
{
    public async ValueTask<VendorDto> Handle(GetVendorQuery query, CancellationToken ct)
    {
        var entity = await context.Vendors
            .Where(x => x.Id == query.Id)
            .Select(x => new VendorDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("Vendor not found");
    }
}
