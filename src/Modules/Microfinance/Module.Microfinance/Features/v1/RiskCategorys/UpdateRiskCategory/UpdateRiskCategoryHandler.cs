using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.RiskCategorys.UpdateRiskCategory;

public record UpdateRiskCategoryCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateRiskCategoryHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateRiskCategoryCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateRiskCategoryCommand command, CancellationToken ct)
    {
        var entity = await context.RiskCategorys.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("RiskCategory not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
