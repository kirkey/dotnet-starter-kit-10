using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Payees.UpdatePayee;

public record UpdatePayeeCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

public class UpdatePayeeHandler(AccountingDbContext context) : ICommandHandler<UpdatePayeeCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdatePayeeCommand command, CancellationToken ct)
    {
        var entity = await context.Payees.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Payee not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
