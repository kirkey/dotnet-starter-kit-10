using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.BranchTargets;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.BranchTargets.GetBranchTarget;

public record GetBranchTargetQuery(Guid Id) : IQuery<BranchTargetDto>;

public class GetBranchTargetHandler(MicrofinanceDbContext context) : IQueryHandler<GetBranchTargetQuery, BranchTargetDto>
{
    public async ValueTask<BranchTargetDto> Handle(GetBranchTargetQuery query, CancellationToken ct)
    {
        var entity = await context.BranchTargets
            .Where(x => x.Id == query.Id)
            .Select(x => new BranchTargetDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("BranchTarget not found");
    }
}
