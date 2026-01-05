using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CustomerCases.DeleteCustomerCase;

namespace FSH.Module.Microfinance.Features.v1.CustomerCases.DeleteCustomerCase;

public class DeleteCustomerCaseHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteCustomerCaseCommand>
{
    public async ValueTask<Unit> Handle(DeleteCustomerCaseCommand command, CancellationToken ct)
    {
        var entity = await context.CustomerCases.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CustomerCase not found");
        
        context.CustomerCases.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
