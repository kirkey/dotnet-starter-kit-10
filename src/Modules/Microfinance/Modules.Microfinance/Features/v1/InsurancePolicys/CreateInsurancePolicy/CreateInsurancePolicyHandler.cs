using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.InsurancePolicys.CreateInsurancePolicy;

public record CreateInsurancePolicyCommand(string Name) : ICommand<Guid>;

public class CreateInsurancePolicyHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateInsurancePolicyCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateInsurancePolicyCommand command, CancellationToken ct)
    {
        var entity = InsurancePolicy.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.InsurancePolicys.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
