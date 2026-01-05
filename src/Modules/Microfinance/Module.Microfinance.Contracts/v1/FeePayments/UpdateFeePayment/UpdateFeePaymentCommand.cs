using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FeePayments.UpdateFeePayment;

public sealed record UpdateFeePaymentCommand(Guid Id, string Name) : ICommand<Guid>;
