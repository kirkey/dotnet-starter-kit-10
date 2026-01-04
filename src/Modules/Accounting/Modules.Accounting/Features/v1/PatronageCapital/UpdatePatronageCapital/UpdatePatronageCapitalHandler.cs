using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.PatronageCapital.UpdatePatronageCapital;

public record UpdatePatronageCapitalCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

public class UpdatePatronageCapitalHandler(AccountingDbContext context) : ICommandHandler<UpdatePatronageCapitalCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdatePatronageCapitalCommand command, CancellationToken ct)
    {
        var entity = await context.PatronageCapital.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("PatronageCapital not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
