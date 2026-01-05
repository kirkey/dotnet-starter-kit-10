using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.KycDocuments.GetKycDocument;
using FSH.Module.Microfinance.Contracts.v1.KycDocuments;

namespace FSH.Module.Microfinance.Features.v1.KycDocuments.GetKycDocument;

public class GetKycDocumentHandler(MicrofinanceDbContext context) : IQueryHandler<GetKycDocumentQuery, KycDocumentDto>
{
    public async ValueTask<KycDocumentDto> Handle(GetKycDocumentQuery query, CancellationToken ct)
    {
        var entity = await context.KycDocuments
            .Where(x => x.Id == query.Id)
            .Select(x => new KycDocumentDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("KycDocument not found");
    }
}
