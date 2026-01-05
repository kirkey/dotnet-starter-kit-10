using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.RiskCategorys.DeleteRiskCategory;

namespace FSH.Module.Microfinance.Features.v1.RiskCategorys.DeleteRiskCategory;

public class DeleteRiskCategoryHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteRiskCategoryCommand>
{
    public async ValueTask<Unit> Handle(DeleteRiskCategoryCommand command, CancellationToken ct)
    {
        var entity = await context.RiskCategorys.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("RiskCategory not found");
        
        context.RiskCategorys.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
