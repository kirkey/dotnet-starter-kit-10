using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Banks.UpdateBank;

public sealed record UpdateBankCommand(
    Guid Id,
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
