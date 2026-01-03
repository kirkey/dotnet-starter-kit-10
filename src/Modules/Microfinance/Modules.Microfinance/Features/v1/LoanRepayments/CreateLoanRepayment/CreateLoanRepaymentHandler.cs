using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.LoanRepayments.CreateLoanRepayment;

public record CreateLoanRepaymentCommand(string Name) : ICommand<Guid>;

public class CreateLoanRepaymentHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateLoanRepaymentCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateLoanRepaymentCommand command, CancellationToken ct)
    {
        var entity = LoanRepayment.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.LoanRepayments.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
