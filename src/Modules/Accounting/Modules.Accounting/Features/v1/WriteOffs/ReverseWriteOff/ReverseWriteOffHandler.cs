// TODO: Implement Reverse operation for WriteOff
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.WriteOffs.ReverseWriteOff;

public record ReverseWriteOffCommand(Guid Id) : ICommand;

public class ReverseWriteOffHandler(AccountingDbContext context) 
    : ICommandHandler<ReverseWriteOffCommand>
{
    public async ValueTask<Unit> Handle(ReverseWriteOffCommand command, CancellationToken ct)
    {
        // TODO: Implement Reverse logic
        throw new NotImplementedException("Reverse operation for WriteOff needs to be implemented");
    }
}
