using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.Loans.UpdateLoan;

public record UpdateLoanCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateLoanHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateLoanCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateLoanCommand command, CancellationToken ct)
    {
        var entity = await context.Loans.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("Loan not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
