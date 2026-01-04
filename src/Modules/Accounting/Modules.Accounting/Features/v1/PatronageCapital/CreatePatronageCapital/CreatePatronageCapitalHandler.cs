using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.PatronageCapital.CreatePatronageCapital;

public record CreatePatronageCapitalCommand(string Name, string? Description) : ICommand<Guid>;

public class CreatePatronageCapitalHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreatePatronageCapitalCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreatePatronageCapitalCommand command, CancellationToken ct)
    {
        var entity = PatronageCapital.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.PatronageCapital.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
