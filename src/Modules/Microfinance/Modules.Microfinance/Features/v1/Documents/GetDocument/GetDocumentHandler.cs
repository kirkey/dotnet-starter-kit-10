using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.Documents;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.Documents.GetDocument;

public record GetDocumentQuery(Guid Id) : IQuery<DocumentDto>;

public class GetDocumentHandler(MicrofinanceDbContext context) : IQueryHandler<GetDocumentQuery, DocumentDto>
{
    public async ValueTask<DocumentDto> Handle(GetDocumentQuery query, CancellationToken ct)
    {
        var entity = await context.Documents
            .Where(x => x.Id == query.Id)
            .Select(x => new DocumentDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("Document not found");
    }
}
