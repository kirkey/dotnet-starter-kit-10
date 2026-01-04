using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Checks.CreateCheck;

public record CreateCheckCommand(
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

public class CreateCheckHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateCheckCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCheckCommand command, CancellationToken ct)
    {
        var entity = Check.Create(
            command.CheckNumber,
            command.CheckDate,
            command.CheckType,
            command.BankAccountId,
            command.AccountNumber,
            command.Amount,
            command.PayeeName,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.PayeeId,
            command.ReferenceNumber,
            command.Notes);

        context.Checks.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}