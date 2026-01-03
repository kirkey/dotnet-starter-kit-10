using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.Loans.CreateLoan;

public record CreateLoanCommand(string Name) : ICommand<Guid>;

public class CreateLoanHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateLoanCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateLoanCommand command, CancellationToken ct)
    {
        var entity = Loan.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.Loans.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
