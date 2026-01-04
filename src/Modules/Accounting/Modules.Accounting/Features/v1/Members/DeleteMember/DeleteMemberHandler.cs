using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.Members.DeleteMember;

public record DeleteMemberCommand(Guid Id) : ICommand;

public class DeleteMemberHandler(AccountingDbContext context) : ICommandHandler<DeleteMemberCommand>
{
    public async ValueTask<Unit> Handle(DeleteMemberCommand command, CancellationToken ct)
    {
        var entity = await context.Members.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Member not found");

        // Business rule: Only inactive members without balances or transaction history can be deleted
        if (entity.IsActive)
            throw new BadRequestException("Cannot delete an active member. Deactivate the member first.");

        var hasInvoices = await context.Invoices.AnyAsync(x => x.MemberId == command.Id, ct).ConfigureAwait(false);
        var hasPayments = await context.Payments.AnyAsync(x => x.MemberId == command.Id, ct).ConfigureAwait(false);
        var hasPatronage = await context.PatronageCapital.AnyAsync(x => x.MemberId == command.Id, ct).ConfigureAwait(false);
        var hasSecurityDeposits = await context.SecurityDeposits.AnyAsync(x => x.MemberId == command.Id, ct).ConfigureAwait(false);
        var hasMeters = await context.Meters.AnyAsync(x => x.MemberId == command.Id, ct).ConfigureAwait(false);

        if (hasInvoices || hasPayments || hasPatronage || hasSecurityDeposits || hasMeters)
            throw new BadRequestException("Cannot delete member with transaction history or assigned resources. Remove associated records first.");
        
        context.Members.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
} 
