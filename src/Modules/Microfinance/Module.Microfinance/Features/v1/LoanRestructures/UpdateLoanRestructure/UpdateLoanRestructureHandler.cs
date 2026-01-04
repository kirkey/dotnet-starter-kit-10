using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.LoanRestructures.UpdateLoanRestructure;

public record UpdateLoanRestructureCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateLoanRestructureHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateLoanRestructureCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateLoanRestructureCommand command, CancellationToken ct)
    {
        var entity = await context.LoanRestructures.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanRestructure not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
