using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.PatronageCapital.DeletePatronageCapital;

public record DeletePatronageCapitalCommand(Guid Id) : ICommand;

public class DeletePatronageCapitalHandler(AccountingDbContext context) : ICommandHandler<DeletePatronageCapitalCommand>
{
    public async ValueTask<Unit> Handle(DeletePatronageCapitalCommand command, CancellationToken ct)
    {
        var entity = await context.PatronageCapital.FindAsync(command.Id, ct)
            ?? throw new FSH.Framework.Core.Exceptions.NotFoundException("PatronageCapital not found");

        // Prevent deletion if any amount has been retired
        if (entity.AmountRetired > 0)
            throw new Accounting.Domain.Exceptions.CannotModifyRetiredPatronageCapitalException(entity.Id);

        context.PatronageCapital.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
