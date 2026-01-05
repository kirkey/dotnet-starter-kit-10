using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.QrPayments.GetQrPayment;

public sealed record GetQrPaymentQuery(Guid Id) : IQuery<QrPaymentDto>;
