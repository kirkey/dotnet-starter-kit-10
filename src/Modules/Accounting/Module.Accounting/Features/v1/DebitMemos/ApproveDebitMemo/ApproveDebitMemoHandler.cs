// TODO: Implement Approve operation for DebitMemo
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.DebitMemos.ApproveDebitMemo;

public record ApproveDebitMemoCommand(Guid Id) : ICommand;

public class ApproveDebitMemoHandler(AccountingDbContext context) 
    : ICommandHandler<ApproveDebitMemoCommand>
{
    public async ValueTask<Unit> Handle(ApproveDebitMemoCommand command, CancellationToken ct)
    {
        // TODO: Implement Approve logic
        throw new NotImplementedException("Approve operation for DebitMemo needs to be implemented");
    }
}
