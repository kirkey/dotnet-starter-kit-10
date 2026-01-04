using FSH.Framework.Core.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Payees.CreatePayee;

public record CreatePayeeCommand(string Name, string? Description) : ICommand<Guid>;

public class CreatePayeeHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreatePayeeCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreatePayeeCommand command, CancellationToken ct)
    {
        var entity = Payee.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.Payees.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
