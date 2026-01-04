using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.Staffs.CreateStaff;

public record CreateStaffCommand(string Name) : ICommand<Guid>;

public class CreateStaffHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateStaffCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateStaffCommand command, CancellationToken ct)
    {
        var entity = Staff.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.Staffs.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
