using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Payments.UpdatePayment;

/// <summary>
/// Update Payment command.
/// </summary>
public record UpdatePaymentCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;
