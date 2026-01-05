// TODO: Implement Reverse operation for WriteOff
using FSH.Module.Accounting.Data;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.WriteOffs.ReverseWriteOff;namespace FSH.Module.Accounting.Features.v1.WriteOffs.ReverseWriteOff;

public class ReverseWriteOffHandler(AccountingDbContext context) 
    : ICommandHandler<ReverseWriteOffCommand>
{
    public async ValueTask<Unit> Handle(ReverseWriteOffCommand command, CancellationToken ct)
    {
        // TODO: Implement Reverse logic
        throw new NotImplementedException("Reverse operation for WriteOff needs to be implemented");
    }
}
