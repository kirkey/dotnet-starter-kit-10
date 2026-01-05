using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.RiskCategorys.CreateRiskCategory;

namespace FSH.Module.Microfinance.Features.v1.RiskCategorys.CreateRiskCategory;

public class CreateRiskCategoryHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateRiskCategoryCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateRiskCategoryCommand command, CancellationToken ct)
    {
        var entity = RiskCategory.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.RiskCategorys.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
