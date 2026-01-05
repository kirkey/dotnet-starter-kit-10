using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanWriteOffs.DeleteLoanWriteOff;

namespace FSH.Module.Microfinance.Features.v1.LoanWriteOffs.DeleteLoanWriteOff;

public class DeleteLoanWriteOffHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteLoanWriteOffCommand>
{
    public async ValueTask<Unit> Handle(DeleteLoanWriteOffCommand command, CancellationToken ct)
    {
        var entity = await context.LoanWriteOffs.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanWriteOff not found");
        
        context.LoanWriteOffs.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
