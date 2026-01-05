// TODO: Implement Approve operation for WriteOff
using FSH.Module.Accounting.Data;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.WriteOffs.ApproveWriteOff;namespace FSH.Module.Accounting.Features.v1.WriteOffs.ApproveWriteOff;

public class ApproveWriteOffHandler(AccountingDbContext context) 
    : ICommandHandler<ApproveWriteOffCommand>
{
    public async ValueTask<Unit> Handle(ApproveWriteOffCommand command, CancellationToken ct)
    {
        // TODO: Implement Approve logic
        throw new NotImplementedException("Approve operation for WriteOff needs to be implemented");
    }
}
