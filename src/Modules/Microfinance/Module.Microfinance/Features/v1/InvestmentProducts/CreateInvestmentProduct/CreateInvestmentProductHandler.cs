using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.InvestmentProducts.CreateInvestmentProduct;

namespace FSH.Module.Microfinance.Features.v1.InvestmentProducts.CreateInvestmentProduct;

public class CreateInvestmentProductHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateInvestmentProductCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateInvestmentProductCommand command, CancellationToken ct)
    {
        var entity = InvestmentProduct.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.InvestmentProducts.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
