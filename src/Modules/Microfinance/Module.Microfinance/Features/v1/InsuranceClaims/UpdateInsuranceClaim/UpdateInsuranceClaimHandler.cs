using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.InsuranceClaims.UpdateInsuranceClaim;

public record UpdateInsuranceClaimCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateInsuranceClaimHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateInsuranceClaimCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateInsuranceClaimCommand command, CancellationToken ct)
    {
        var entity = await context.InsuranceClaims.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("InsuranceClaim not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
