using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FeePayments.CreateFeePayment;

public sealed record CreateFeePaymentCommand(string Name) : ICommand<Guid>;
