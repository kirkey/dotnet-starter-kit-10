using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.LoanOfficerTargets.DeleteLoanOfficerTarget;

public record DeleteLoanOfficerTargetCommand(Guid Id) : ICommand;

public class DeleteLoanOfficerTargetHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteLoanOfficerTargetCommand>
{
    public async ValueTask<Unit> Handle(DeleteLoanOfficerTargetCommand command, CancellationToken ct)
    {
        var entity = await context.LoanOfficerTargets.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanOfficerTarget not found");
        
        context.LoanOfficerTargets.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
