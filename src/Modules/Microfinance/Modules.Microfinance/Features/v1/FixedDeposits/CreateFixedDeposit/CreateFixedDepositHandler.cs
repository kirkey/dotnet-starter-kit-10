using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.FixedDeposits.CreateFixedDeposit;

public record CreateFixedDepositCommand(string Name) : ICommand<Guid>;

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
