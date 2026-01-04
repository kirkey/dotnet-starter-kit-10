using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.ApprovalRequests;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.ApprovalRequests.GetApprovalRequest;

public record GetApprovalRequestQuery(Guid Id) : IQuery<ApprovalRequestDto>;

public class GetApprovalRequestHandler(MicrofinanceDbContext context) : IQueryHandler<GetApprovalRequestQuery, ApprovalRequestDto>
{
    public async ValueTask<ApprovalRequestDto> Handle(GetApprovalRequestQuery query, CancellationToken ct)
    {
        var entity = await context.ApprovalRequests
            .Where(x => x.Id == query.Id)
            .Select(x => new ApprovalRequestDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("ApprovalRequest not found");
    }
}
