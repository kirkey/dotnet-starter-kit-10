// TODO: Implement Refund operation for SecurityDeposit
using FSH.Module.Accounting.Data;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.SecurityDeposits.RefundSecurityDeposit;

namespace FSH.Module.Accounting.Features.v1.SecurityDeposits.RefundSecurityDeposit;

public class RefundSecurityDepositHandler(AccountingDbContext context) 
    : ICommandHandler<RefundSecurityDepositCommand>
{
    public async ValueTask<Unit> Handle(RefundSecurityDepositCommand command, CancellationToken ct)
    {
        // TODO: Implement Refund logic
        throw new NotImplementedException("Refund operation for SecurityDeposit needs to be implemented");
    }
}
