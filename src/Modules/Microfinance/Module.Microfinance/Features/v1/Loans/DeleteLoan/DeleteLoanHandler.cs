using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.Loans.DeleteLoan;

public record DeleteLoanCommand(Guid Id) : ICommand;

public class DeleteLoanHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteLoanCommand>
{
    public async ValueTask<Unit> Handle(DeleteLoanCommand command, CancellationToken ct)
    {
        var entity = await context.Loans.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("Loan not found");
        
        context.Loans.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
