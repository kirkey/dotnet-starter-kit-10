using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.PostingBatches;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.PostingBatches.GetPostingBatch;

public record GetPostingBatchQuery(Guid Id) : IQuery<PostingBatchDto>;

public class GetPostingBatchHandler(AccountingDbContext context) : IQueryHandler<GetPostingBatchQuery, PostingBatchDto>
{
    public async ValueTask<PostingBatchDto> Handle(GetPostingBatchQuery query, CancellationToken ct)
    {
        var entity = await context.PostingBatches
            .Where(x => x.Id == query.Id)
            .Select(x => new PostingBatchDto(
                x.Id,
                x.Name,
                x.BatchDate,
                x.Description,
                x.Status,
                x.TotalDebits,
                x.TotalCredits,
                x.EntryCount,
                x.PostedOn,
                x.PostedBy,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("PostingBatch not found");
    }
}
