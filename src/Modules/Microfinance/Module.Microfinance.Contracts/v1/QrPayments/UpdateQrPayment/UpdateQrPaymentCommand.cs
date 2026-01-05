using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.QrPayments.UpdateQrPayment;

public sealed record UpdateQrPaymentCommand(Guid Id, string Name) : ICommand<Guid>;
