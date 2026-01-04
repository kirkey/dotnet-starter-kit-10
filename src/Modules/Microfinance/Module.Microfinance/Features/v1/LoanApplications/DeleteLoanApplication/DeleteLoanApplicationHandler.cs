using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.LoanApplications.DeleteLoanApplication;

public record DeleteLoanApplicationCommand(Guid Id) : ICommand;

public class DeleteLoanApplicationHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteLoanApplicationCommand>
{
    public async ValueTask<Unit> Handle(DeleteLoanApplicationCommand command, CancellationToken ct)
    {
        var entity = await context.LoanApplications.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanApplication not found");
        
        context.LoanApplications.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
