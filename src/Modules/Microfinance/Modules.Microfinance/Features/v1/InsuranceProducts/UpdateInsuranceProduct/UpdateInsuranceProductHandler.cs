using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.InsuranceProducts.UpdateInsuranceProduct;

public record UpdateInsuranceProductCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateInsuranceProductHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateInsuranceProductCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateInsuranceProductCommand command, CancellationToken ct)
    {
        var entity = await context.InsuranceProducts.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("InsuranceProduct not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
