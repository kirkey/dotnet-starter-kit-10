using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.QrPayments.CreateQrPayment;

public sealed record CreateQrPaymentCommand(string Name) : ICommand<Guid>;
