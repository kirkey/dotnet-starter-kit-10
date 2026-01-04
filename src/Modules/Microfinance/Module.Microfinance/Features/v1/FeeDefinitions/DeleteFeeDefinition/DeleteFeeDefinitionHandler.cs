using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.FeeDefinitions.DeleteFeeDefinition;

public record DeleteFeeDefinitionCommand(Guid Id) : ICommand;

public class DeleteFeeDefinitionHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteFeeDefinitionCommand>
{
    public async ValueTask<Unit> Handle(DeleteFeeDefinitionCommand command, CancellationToken ct)
    {
        var entity = await context.FeeDefinitions.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("FeeDefinition not found");
        
        context.FeeDefinitions.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
