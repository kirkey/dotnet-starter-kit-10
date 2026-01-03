using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.KycDocuments.DeleteKycDocument;

public record DeleteKycDocumentCommand(Guid Id) : ICommand;

public class DeleteKycDocumentHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteKycDocumentCommand>
{
    public async ValueTask<Unit> Handle(DeleteKycDocumentCommand command, CancellationToken ct)
    {
        var entity = await context.KycDocuments.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("KycDocument not found");
        
        context.KycDocuments.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
