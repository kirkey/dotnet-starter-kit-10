using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.KycDocuments.UpdateKycDocument;

public record UpdateKycDocumentCommand(Guid Id, string Name) : ICommand<Guid>;

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
