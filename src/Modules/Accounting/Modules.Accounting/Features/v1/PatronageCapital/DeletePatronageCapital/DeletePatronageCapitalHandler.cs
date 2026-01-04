using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.PatronageCapital.DeletePatronageCapital;

public record DeletePatronageCapitalCommand(Guid Id) : ICommand;

public class DeletePatronageCapitalHandler(AccountingDbContext context) : ICommandHandler<DeletePatronageCapitalCommand>
{
    public async ValueTask<Unit> Handle(DeletePatronageCapitalCommand command, CancellationToken ct)
    {
        var entity = await context.PatronageCapital.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("PatronageCapital not found");
        
        context.PatronageCapital.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
