using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.CommunicationTemplates.CreateCommunicationTemplate;

public record CreateCommunicationTemplateCommand(string Name) : ICommand<Guid>;

public class CreateCommunicationTemplateHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateCommunicationTemplateCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCommunicationTemplateCommand command, CancellationToken ct)
    {
        var entity = CommunicationTemplate.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.CommunicationTemplates.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
