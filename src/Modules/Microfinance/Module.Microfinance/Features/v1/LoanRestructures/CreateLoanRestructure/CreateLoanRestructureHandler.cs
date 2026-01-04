using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.LoanRestructures.CreateLoanRestructure;

public record CreateLoanRestructureCommand(string Name) : ICommand<Guid>;

public class CreateLoanRestructureHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateLoanRestructureCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateLoanRestructureCommand command, CancellationToken ct)
    {
        var entity = LoanRestructure.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.LoanRestructures.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
