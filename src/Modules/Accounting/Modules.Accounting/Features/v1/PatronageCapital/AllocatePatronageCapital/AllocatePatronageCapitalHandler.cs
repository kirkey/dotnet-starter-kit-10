// TODO: Implement Allocate operation for PatronageCapital
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.PatronageCapital.AllocatePatronageCapital;

public record AllocatePatronageCapitalCommand(Guid Id) : ICommand;

public class AllocatePatronageCapitalHandler(AccountingDbContext context) 
    : ICommandHandler<AllocatePatronageCapitalCommand>
{
    public async ValueTask<Unit> Handle(AllocatePatronageCapitalCommand command, CancellationToken ct)
    {
        // TODO: Implement Allocate logic
        throw new NotImplementedException("Allocate operation for PatronageCapital needs to be implemented");
    }
}
