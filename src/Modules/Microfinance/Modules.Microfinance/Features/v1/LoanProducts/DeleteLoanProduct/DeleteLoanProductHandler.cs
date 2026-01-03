using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.LoanProducts.DeleteLoanProduct;

public record DeleteLoanProductCommand(Guid Id) : ICommand;

public class DeleteLoanProductHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteLoanProductCommand>
{
    public async ValueTask<Unit> Handle(DeleteLoanProductCommand command, CancellationToken ct)
    {
        var entity = await context.LoanProducts.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanProduct not found");
        
        context.LoanProducts.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
