using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.KycDocuments.UpdateKycDocument;

namespace FSH.Module.Microfinance.Features.v1.KycDocuments.UpdateKycDocument;

public class UpdateKycDocumentHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateKycDocumentCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateKycDocumentCommand command, CancellationToken ct)
    {
        var entity = await context.KycDocuments.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("KycDocument not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
