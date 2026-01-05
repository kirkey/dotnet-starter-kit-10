using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.CreditMemos.CreateCreditMemo;

namespace FSH.Module.Accounting.Features.v1.CreditMemos.CreateCreditMemo;

public class CreateCreditMemoHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateCreditMemoCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCreditMemoCommand command, CancellationToken ct)
    {
        var entity = CreditMemo.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.CreditMemos.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
