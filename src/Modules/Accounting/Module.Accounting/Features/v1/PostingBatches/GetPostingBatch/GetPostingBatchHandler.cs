using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.PostingBatches;
using FSH.Module.Accounting.Contracts.v1.PostingBatches.GetPostingBatch;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.PostingBatches.GetPostingBatch;

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
