using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Payments.CreatePayment;

/// <summary>
/// Create Payment command.
/// </summary>
public record CreatePaymentCommand(string Name, string? Description) : ICommand<Guid>;
