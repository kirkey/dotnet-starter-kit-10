using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Banks.UpdateBank;

public record UpdateBankCommand(
    Guid Id,
    string BankName,
    string? BankCode = null,
    string? Address = null,
    string? ContactName = null,
    string? ContactPhone = null,
    string? RoutingNumber = null,
    string? SwiftCode = null,
    string? CurrencyCode = null,
    decimal? OpeningBalance = null,
    bool? IsDefault = null,
    string? Description = null) : ICommand<Guid>;

public class UpdateBankHandler(AccountingDbContext context) : ICommandHandler<UpdateBankCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateBankCommand command, CancellationToken ct)
    {
        var entity = await context.Banks.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Bank not found");
        
        entity.Update(
            command.BankName,
            command.BankCode,
            command.Address,
            command.ContactName,
            command.ContactPhone,
            command.RoutingNumber,
            command.SwiftCode,
            command.CurrencyCode,
            command.OpeningBalance,
            command.IsDefault,
            command.Description);
        
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
