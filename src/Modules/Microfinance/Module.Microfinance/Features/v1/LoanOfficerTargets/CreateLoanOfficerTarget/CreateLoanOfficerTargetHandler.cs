using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.LoanOfficerTargets.CreateLoanOfficerTarget;

public record CreateLoanOfficerTargetCommand(string Name) : ICommand<Guid>;

public class CreateLoanOfficerTargetHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateLoanOfficerTargetCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateLoanOfficerTargetCommand command, CancellationToken ct)
    {
        var entity = LoanOfficerTarget.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.LoanOfficerTargets.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
