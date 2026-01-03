using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.RiskCategorys.CreateRiskCategory;

public record CreateRiskCategoryCommand(string Name) : ICommand<Guid>;

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
