using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.InvestmentProducts.DeleteInvestmentProduct;

public record DeleteInvestmentProductCommand(Guid Id) : ICommand;

public class DeleteInvestmentProductHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteInvestmentProductCommand>
{
    public async ValueTask<Unit> Handle(DeleteInvestmentProductCommand command, CancellationToken ct)
    {
        var entity = await context.InvestmentProducts.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("InvestmentProduct not found");
        
        context.InvestmentProducts.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
