using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Payments.DeletePayment;

/// <summary>
/// Delete Payment command.
/// </summary>
public record DeletePaymentCommand(Guid Id) : ICommand;