using FSH.Framework.Core.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.TaxCodes.CreateTaxCode;

public record CreateTaxCodeCommand(string Name, string? Description) : ICommand<Guid>;

public class CreateTaxCodeHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateTaxCodeCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateTaxCodeCommand command, CancellationToken ct)
    {
        var entity = TaxCode.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.TaxCodes.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
