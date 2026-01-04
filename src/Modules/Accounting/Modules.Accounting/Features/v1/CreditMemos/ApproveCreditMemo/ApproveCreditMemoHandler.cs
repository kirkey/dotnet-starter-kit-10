// TODO: Implement Approve operation for CreditMemo
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.CreditMemos.ApproveCreditMemo;

public record ApproveCreditMemoCommand(Guid Id) : ICommand;

public class ApproveCreditMemoHandler(AccountingDbContext context) 
    : ICommandHandler<ApproveCreditMemoCommand>
{
    public async ValueTask<Unit> Handle(ApproveCreditMemoCommand command, CancellationToken ct)
    {
        // TODO: Implement Approve logic
        throw new NotImplementedException("Approve operation for CreditMemo needs to be implemented");
    }
}
