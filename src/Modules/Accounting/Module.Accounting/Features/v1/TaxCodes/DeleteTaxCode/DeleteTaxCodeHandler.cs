using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.TaxCodes.DeleteTaxCode;

public record DeleteTaxCodeCommand(Guid Id) : ICommand;

public class DeleteTaxCodeHandler(AccountingDbContext context) : ICommandHandler<DeleteTaxCodeCommand>
{
    public async ValueTask<Unit> Handle(DeleteTaxCodeCommand command, CancellationToken ct)
    {
        var entity = await context.TaxCodes.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("TaxCode not found");
        
        context.TaxCodes.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
