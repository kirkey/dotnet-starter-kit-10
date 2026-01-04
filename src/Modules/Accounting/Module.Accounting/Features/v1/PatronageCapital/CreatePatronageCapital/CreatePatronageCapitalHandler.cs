using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using PatronageCapitalEntity = FSH.Module.Accounting.Domain.PatronageCapital;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.PatronageCapital.CreatePatronageCapital;

public record CreatePatronageCapitalCommand(string Name, string? Description) : ICommand<Guid>;

public class CreatePatronageCapitalHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreatePatronageCapitalCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreatePatronageCapitalCommand command, CancellationToken ct)
    {
        var entity = PatronageCapitalEntity.Create(
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
