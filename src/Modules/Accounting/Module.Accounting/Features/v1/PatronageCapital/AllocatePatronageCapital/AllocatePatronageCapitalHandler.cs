// TODO: Implement Allocate operation for PatronageCapital
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.PatronageCapital.AllocatePatronageCapital;

public record AllocatePatronageCapitalCommand(Guid Id) : ICommand;

public class AllocatePatronageCapitalHandler(AccountingDbContext context) 
    : ICommandHandler<AllocatePatronageCapitalCommand>
{
    public async ValueTask<Unit> Handle(AllocatePatronageCapitalCommand command, CancellationToken ct)
    {
        // Validate entity exists
        var entity = await context.PatronageCapital.FindAsync(command.Id, ct)
            ?? throw new Accounting.Domain.Exceptions.PatronageCapitalByIdNotFoundException(command.Id);

        // Validate member eligibility
        var member = await context.Members.FindAsync(entity.MemberId, ct)
            ?? throw new FSH.Framework.Core.Exceptions.NotFoundException("Member not found");

        if (!member.IsActive)
            throw new Accounting.Domain.Exceptions.MemberNotEligibleForPatronageCapitalException(entity.MemberId);

        // Validate amounts/status
        if (entity.AmountAllocated <= 0)
            throw new Accounting.Domain.Exceptions.InvalidPatronageCapitalAmountException();

        if (entity.AmountRetired >= entity.AmountAllocated)
            throw new Accounting.Domain.Exceptions.CannotModifyRetiredPatronageCapitalException(entity.Id);

        // No-op state change for now (allocation may create accounting entries externally)
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
