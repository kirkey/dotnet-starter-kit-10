using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.LoanWriteOffs.UpdateLoanWriteOff;

public record UpdateLoanWriteOffCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateLoanWriteOffHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateLoanWriteOffCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateLoanWriteOffCommand command, CancellationToken ct)
    {
        var entity = await context.LoanWriteOffs.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("LoanWriteOff not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
