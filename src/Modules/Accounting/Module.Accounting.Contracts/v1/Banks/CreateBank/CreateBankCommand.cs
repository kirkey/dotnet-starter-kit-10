using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Banks.CreateBank;

public sealed record CreateBankCommand(
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