using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FeePayments.DeleteFeePayment;

public sealed record DeleteFeePaymentCommand(Guid Id) : ICommand;
