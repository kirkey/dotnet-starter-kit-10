using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.Staffs.DeleteStaff;

public record DeleteStaffCommand(Guid Id) : ICommand;

public class DeleteStaffHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteStaffCommand>
{
    public async ValueTask<Unit> Handle(DeleteStaffCommand command, CancellationToken ct)
    {
        var entity = await context.Staffs.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("Staff not found");
        
        context.Staffs.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
