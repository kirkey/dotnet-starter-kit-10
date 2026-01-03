using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.CustomerCases.CreateCustomerCase;

public record CreateCustomerCaseCommand(string Name) : ICommand<Guid>;

public class CreateCustomerCaseHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateCustomerCaseCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCustomerCaseCommand command, CancellationToken ct)
    {
        var entity = CustomerCase.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.CustomerCases.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
