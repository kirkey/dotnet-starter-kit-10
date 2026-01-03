using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.Branches;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.Branches.GetBranch;

public record GetBranchQuery(Guid Id) : IQuery<BranchDto>;

public class GetBranchHandler(MicrofinanceDbContext context) : IQueryHandler<GetBranchQuery, BranchDto>
{
    public async ValueTask<BranchDto> Handle(GetBranchQuery query, CancellationToken ct)
    {
        var entity = await context.Branches
            .Where(x => x.Id == query.Id)
            .Select(x => new BranchDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("Branch not found");
    }
}
