using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.Vendors;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.Vendors.GetVendor;

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
