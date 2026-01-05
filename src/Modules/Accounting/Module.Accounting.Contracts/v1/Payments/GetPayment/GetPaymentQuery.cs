using FSH.Module.Accounting.Contracts.v1.Payments;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Payments.GetPayment;

/// <summary>
/// Get Payment query.
/// </summary>
public record GetPaymentQuery(Guid Id) : IQuery<PaymentDto>;