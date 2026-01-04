using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.Documents.UpdateDocument;

public record UpdateDocumentCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateDocumentHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateDocumentCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateDocumentCommand command, CancellationToken ct)
    {
        var entity = await context.Documents.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("Document not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
