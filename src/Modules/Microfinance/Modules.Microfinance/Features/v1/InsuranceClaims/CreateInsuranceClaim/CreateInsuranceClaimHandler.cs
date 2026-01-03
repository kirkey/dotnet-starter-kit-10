using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.InsuranceClaims.CreateInsuranceClaim;

public record CreateInsuranceClaimCommand(string Name) : ICommand<Guid>;

public class CreateInsuranceClaimHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateInsuranceClaimCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateInsuranceClaimCommand command, CancellationToken ct)
    {
        var entity = InsuranceClaim.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.InsuranceClaims.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
