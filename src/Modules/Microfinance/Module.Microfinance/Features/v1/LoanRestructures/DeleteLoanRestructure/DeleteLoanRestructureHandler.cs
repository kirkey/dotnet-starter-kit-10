using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.LoanRestructures.DeleteLoanRestructure;

public record DeleteLoanRestructureCommand(Guid Id) : ICommand;

public class DeleteLoanRestructureHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteLoanRestructureCommand>
{
    public async ValueTask<Unit> Handle(DeleteLoanRestructureCommand command, CancellationToken ct)
    {
        var entity = await context.LoanRestructures.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanRestructure not found");
        
        context.LoanRestructures.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
