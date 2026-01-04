using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Banks.CreateBank;

public record CreateBankCommand(
    string BankName,
    string? BankCode = null,
    string? Address = null,
    string? ContactName = null,
    string? ContactPhone = null,
    string? RoutingNumber = null,
    string? SwiftCode = null,
    string? CurrencyCode = null,
    decimal OpeningBalance = 0,
    bool IsDefault = false,
    string? Description = null) : ICommand<Guid>;

public class CreateBankHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateBankCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateBankCommand command, CancellationToken ct)
    {
        var entity = Bank.Create(
            command.BankName,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
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
        
        context.Banks.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
