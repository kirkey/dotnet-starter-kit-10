using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.LoanOfficerTargets.UpdateLoanOfficerTarget;

public record UpdateLoanOfficerTargetCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateLoanOfficerTargetHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateLoanOfficerTargetCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateLoanOfficerTargetCommand command, CancellationToken ct)
    {
        var entity = await context.LoanOfficerTargets.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanOfficerTarget not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
