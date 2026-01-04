using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.Documents.CreateDocument;

public record CreateDocumentCommand(string Name) : ICommand<Guid>;

public class CreateDocumentHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateDocumentCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateDocumentCommand command, CancellationToken ct)
    {
        var entity = Document.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.Documents.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
