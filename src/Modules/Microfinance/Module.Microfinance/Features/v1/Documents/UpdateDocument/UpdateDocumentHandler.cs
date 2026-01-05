using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.Documents.UpdateDocument;

namespace FSH.Module.Microfinance.Features.v1.Documents.UpdateDocument;

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
