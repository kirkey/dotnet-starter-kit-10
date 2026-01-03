using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.Documents.DeleteDocument;

public record DeleteDocumentCommand(Guid Id) : ICommand;

public class DeleteDocumentHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteDocumentCommand>
{
    public async ValueTask<Unit> Handle(DeleteDocumentCommand command, CancellationToken ct)
    {
        var entity = await context.Documents.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("Document not found");
        
        context.Documents.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
