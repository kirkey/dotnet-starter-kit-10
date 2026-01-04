using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.LoanDisbursementTranches.CreateLoanDisbursementTranche;

public record CreateLoanDisbursementTrancheCommand(string Name) : ICommand<Guid>;

public class CreateLoanDisbursementTrancheHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateLoanDisbursementTrancheCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateLoanDisbursementTrancheCommand command, CancellationToken ct)
    {
        var entity = LoanDisbursementTranche.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.LoanDisbursementTranches.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
