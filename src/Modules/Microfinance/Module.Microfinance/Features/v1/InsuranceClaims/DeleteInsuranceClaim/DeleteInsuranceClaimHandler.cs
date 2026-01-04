using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.InsuranceClaims.DeleteInsuranceClaim;

public record DeleteInsuranceClaimCommand(Guid Id) : ICommand;

public class DeleteInsuranceClaimHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteInsuranceClaimCommand>
{
    public async ValueTask<Unit> Handle(DeleteInsuranceClaimCommand command, CancellationToken ct)
    {
        var entity = await context.InsuranceClaims.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("InsuranceClaim not found");
        
        context.InsuranceClaims.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
