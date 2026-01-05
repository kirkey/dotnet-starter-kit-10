// TODO: Implement Approve operation for CreditMemo
using FSH.Module.Accounting.Data;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.CreditMemos.ApproveCreditMemo;

namespace FSH.Module.Accounting.Features.v1.CreditMemos.ApproveCreditMemo;

public class ApproveCreditMemoHandler(AccountingDbContext context) 
    : ICommandHandler<ApproveCreditMemoCommand>
{
    public async ValueTask<Unit> Handle(ApproveCreditMemoCommand command, CancellationToken ct)
    {
        // TODO: Implement Approve logic
        throw new NotImplementedException("Approve operation for CreditMemo needs to be implemented");
    }
}
