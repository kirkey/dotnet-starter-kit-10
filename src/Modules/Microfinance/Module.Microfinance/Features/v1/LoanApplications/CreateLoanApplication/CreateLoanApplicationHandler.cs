using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.LoanApplications.CreateLoanApplication;

public record CreateLoanApplicationCommand(string Name) : ICommand<Guid>;

public class CreateLoanApplicationHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateLoanApplicationCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateLoanApplicationCommand command, CancellationToken ct)
    {
        var entity = LoanApplication.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.LoanApplications.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
