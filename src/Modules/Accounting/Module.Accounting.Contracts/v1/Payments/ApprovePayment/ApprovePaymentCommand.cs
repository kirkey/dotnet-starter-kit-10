using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Payments.ApprovePayment;

/// <summary>
/// Approve Payment command.
/// </summary>
public record ApprovePaymentCommand(Guid Id) : ICommand;