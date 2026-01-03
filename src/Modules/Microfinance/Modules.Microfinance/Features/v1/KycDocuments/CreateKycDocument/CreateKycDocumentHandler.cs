using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.KycDocuments.CreateKycDocument;

public record CreateKycDocumentCommand(string Name) : ICommand<Guid>;

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
