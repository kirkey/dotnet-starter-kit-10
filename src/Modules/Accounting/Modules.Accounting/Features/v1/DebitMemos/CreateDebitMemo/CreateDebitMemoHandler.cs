using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.DebitMemos.CreateDebitMemo;

public record CreateDebitMemoCommand(string Name, string? Description) : ICommand<Guid>;

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
