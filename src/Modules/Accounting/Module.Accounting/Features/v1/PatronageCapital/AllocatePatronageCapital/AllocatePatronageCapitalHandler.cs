// TODO: Implement Allocate operation for PatronageCapital
using FSH.Module.Accounting.Data;
using Mediator;
using FSH.Framework.Core.Exceptions;

namespace FSH.Module.Accounting.Features.v1.PatronageCapital.AllocatePatronageCapital;

public record AllocatePatronageCapitalCommand(Guid Id) : ICommand;

public class AllocatePatronageCapitalHandler(AccountingDbContext context) 
    : ICommandHandler<AllocatePatronageCapitalCommand>
{
    public async ValueTask<Unit> Handle(AllocatePatronageCapitalCommand command, CancellationToken ct)
    {
        // Validate entity exists
        var entity = await context.PatronageCapital.FindAsync(command.Id, ct)
            ?? throw new NotFoundException($"PatronageCapital {command.Id} not found");

        // Validate member eligibility
        var member = await context.Members.FindAsync(entity.MemberId, ct)
            ?? throw new NotFoundException("Member not found");

        if (!member.IsActive)
            throw new BadRequestException("Member not eligible for patronage capital allocation");

        // Validate amounts/status
        if (entity.AmountAllocated <= 0)
            throw new BadRequestException("Invalid patronage capital amount");

        if (entity.AmountRetired >= entity.AmountAllocated)
            throw new BadRequestException("Cannot modify retired patronage capital");
        // No-op state change for now (allocation may create accounting entries externally)
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
