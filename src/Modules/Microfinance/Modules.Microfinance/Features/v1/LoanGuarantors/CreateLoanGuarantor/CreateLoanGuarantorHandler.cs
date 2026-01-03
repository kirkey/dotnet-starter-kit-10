using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.LoanGuarantors.CreateLoanGuarantor;

public record CreateLoanGuarantorCommand(string Name) : ICommand<Guid>;

public class CreateLoanGuarantorHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateLoanGuarantorCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateLoanGuarantorCommand command, CancellationToken ct)
    {
        var entity = LoanGuarantor.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.LoanGuarantors.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
