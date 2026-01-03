using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.RiskCategorys.DeleteRiskCategory;

public record DeleteRiskCategoryCommand(Guid Id) : ICommand;

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
