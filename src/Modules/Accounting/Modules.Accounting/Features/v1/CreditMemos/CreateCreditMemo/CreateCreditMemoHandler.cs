using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.CreditMemos.CreateCreditMemo;

public record CreateCreditMemoCommand(string Name, string? Description) : ICommand<Guid>;

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
