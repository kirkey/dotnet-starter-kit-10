using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CommunicationTemplates.DeleteCommunicationTemplate;

public record DeleteCommunicationTemplateCommand(Guid Id) : ICommand;

public class DeleteCommunicationTemplateHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteCommunicationTemplateCommand>
{
    public async ValueTask<Unit> Handle(DeleteCommunicationTemplateCommand command, CancellationToken ct)
    {
        var entity = await context.CommunicationTemplates.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CommunicationTemplate not found");
        
        context.CommunicationTemplates.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
