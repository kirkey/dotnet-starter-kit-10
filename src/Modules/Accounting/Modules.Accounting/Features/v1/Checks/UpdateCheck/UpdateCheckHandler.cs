using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Checks.UpdateCheck;

public record UpdateCheckCommand(
    Guid Id,
    string CheckNumber,
    DateTime CheckDate,
    string CheckType,
    Guid BankAccountId,
    string AccountNumber,
    decimal Amount,
    string PayeeName,
    Guid? PayeeId = null,
    string? ReferenceNumber = null,
    string? Notes = null) : ICommand<Guid>;

public class UpdateCheckHandler(AccountingDbContext context) : ICommandHandler<UpdateCheckCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateCheckCommand command, CancellationToken ct)
    {
        var entity = await context.Checks.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Check not found");

        entity.Update(
            command.CheckNumber,
            command.CheckDate,
            command.CheckType,
            command.BankAccountId,
            command.AccountNumber,
            command.Amount,
            command.PayeeName,
            command.PayeeId,
            command.ReferenceNumber,
            command.Notes);

        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}