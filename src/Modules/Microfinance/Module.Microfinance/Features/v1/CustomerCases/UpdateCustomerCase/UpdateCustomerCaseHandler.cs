using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CustomerCases.UpdateCustomerCase;

namespace FSH.Module.Microfinance.Features.v1.CustomerCases.UpdateCustomerCase;

public class UpdateCustomerCaseHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateCustomerCaseCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateCustomerCaseCommand command, CancellationToken ct)
    {
        var entity = await context.CustomerCases.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CustomerCase not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
