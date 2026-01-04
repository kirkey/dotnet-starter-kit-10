using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.TaxCodes.UpdateTaxCode;

public record UpdateTaxCodeCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

public class UpdateTaxCodeHandler(AccountingDbContext context) : ICommandHandler<UpdateTaxCodeCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateTaxCodeCommand command, CancellationToken ct)
    {
        var entity = await context.TaxCodes.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("TaxCode not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
