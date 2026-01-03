using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.LoanWriteOffs.DeleteLoanWriteOff;

public record DeleteLoanWriteOffCommand(Guid Id) : ICommand;

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
