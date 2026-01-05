using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.Documents.GetDocument;
using FSH.Module.Microfinance.Contracts.v1.Documents;

namespace FSH.Module.Microfinance.Features.v1.Documents.GetDocument;

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
