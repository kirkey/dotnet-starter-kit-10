using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.LoanProducts.CreateLoanProduct;

public record CreateLoanProductCommand(string Name) : ICommand<Guid>;

public class CreateLoanProductHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateLoanProductCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateLoanProductCommand command, CancellationToken ct)
    {
        var entity = LoanProduct.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.LoanProducts.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
