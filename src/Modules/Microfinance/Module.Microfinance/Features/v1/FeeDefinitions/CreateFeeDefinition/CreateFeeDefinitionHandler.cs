using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.FeeDefinitions.CreateFeeDefinition;

namespace FSH.Module.Microfinance.Features.v1.FeeDefinitions.CreateFeeDefinition;

public class CreateFeeDefinitionHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateFeeDefinitionCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateFeeDefinitionCommand command, CancellationToken ct)
    {
        var entity = FeeDefinition.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.FeeDefinitions.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
