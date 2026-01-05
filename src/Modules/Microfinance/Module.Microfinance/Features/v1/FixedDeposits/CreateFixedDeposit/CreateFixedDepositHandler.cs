using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.FixedDeposits.CreateFixedDeposit;

namespace FSH.Module.Microfinance.Features.v1.FixedDeposits.CreateFixedDeposit;

public class CreateFixedDepositHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateFixedDepositCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateFixedDepositCommand command, CancellationToken ct)
    {
        var entity = FixedDeposit.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.FixedDeposits.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
