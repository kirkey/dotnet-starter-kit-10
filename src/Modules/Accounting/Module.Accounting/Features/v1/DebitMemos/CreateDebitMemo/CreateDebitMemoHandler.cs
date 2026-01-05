using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.DebitMemos.CreateDebitMemo;

namespace FSH.Module.Accounting.Features.v1.DebitMemos.CreateDebitMemo;

public class CreateDebitMemoHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateDebitMemoCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateDebitMemoCommand command, CancellationToken ct)
    {
        var entity = DebitMemo.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.DebitMemos.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
