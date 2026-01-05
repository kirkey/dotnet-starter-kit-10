// TODO: Implement Reverse operation for FiscalPeriodClose
using FSH.Module.Accounting.Data;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.FiscalPeriodClose.ReverseFiscalPeriodClose;

namespace FSH.Module.Accounting.Features.v1.FiscalPeriodClose.ReverseFiscalPeriodClose;

public class ReverseFiscalPeriodCloseHandler(AccountingDbContext context) 
    : ICommandHandler<ReverseFiscalPeriodCloseCommand>
{
    public async ValueTask<Unit> Handle(ReverseFiscalPeriodCloseCommand command, CancellationToken ct)
    {
        // TODO: Implement Reverse logic
        throw new NotImplementedException("Reverse operation for FiscalPeriodClose needs to be implemented");
    }
}
