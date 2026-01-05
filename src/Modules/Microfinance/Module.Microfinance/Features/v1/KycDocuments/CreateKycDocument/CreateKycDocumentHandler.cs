using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.KycDocuments.CreateKycDocument;

namespace FSH.Module.Microfinance.Features.v1.KycDocuments.CreateKycDocument;

public class CreateKycDocumentHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateKycDocumentCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateKycDocumentCommand command, CancellationToken ct)
    {
        var entity = KycDocument.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.KycDocuments.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
