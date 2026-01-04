using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.CreditMemos.UpdateCreditMemo;

public record UpdateCreditMemoCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

public class UpdateCreditMemoHandler(AccountingDbContext context) : ICommandHandler<UpdateCreditMemoCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateCreditMemoCommand command, CancellationToken ct)
    {
        var entity = await context.CreditMemos.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("CreditMemo not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
