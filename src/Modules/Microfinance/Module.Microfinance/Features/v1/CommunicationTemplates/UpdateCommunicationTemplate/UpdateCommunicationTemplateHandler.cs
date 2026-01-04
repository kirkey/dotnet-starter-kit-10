using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CommunicationTemplates.UpdateCommunicationTemplate;

public record UpdateCommunicationTemplateCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateCommunicationTemplateHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateCommunicationTemplateCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateCommunicationTemplateCommand command, CancellationToken ct)
    {
        var entity = await context.CommunicationTemplates.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CommunicationTemplate not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
