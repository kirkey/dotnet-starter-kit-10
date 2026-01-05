using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FeePayments.GetFeePayment;

public sealed record GetFeePaymentQuery(Guid Id) : IQuery<FeePaymentDto>;
