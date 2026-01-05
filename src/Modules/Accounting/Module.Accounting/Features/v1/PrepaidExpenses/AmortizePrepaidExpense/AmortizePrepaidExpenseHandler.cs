// TODO: Implement Amortize operation for PrepaidExpense
using FSH.Module.Accounting.Data;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.PrepaidExpenses.AmortizePrepaidExpense;

namespace FSH.Module.Accounting.Features.v1.PrepaidExpenses.AmortizePrepaidExpense;

public class AmortizePrepaidExpenseHandler(AccountingDbContext context) 
    : ICommandHandler<AmortizePrepaidExpenseCommand>
{
    public async ValueTask<Unit> Handle(AmortizePrepaidExpenseCommand command, CancellationToken ct)
    {
        // TODO: Implement Amortize logic
        throw new NotImplementedException("Amortize operation for PrepaidExpense needs to be implemented");
    }
}
