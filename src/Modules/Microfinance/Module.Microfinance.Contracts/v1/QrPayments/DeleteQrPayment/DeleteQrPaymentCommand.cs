using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.QrPayments.DeleteQrPayment;

public sealed record DeleteQrPaymentCommand(Guid Id) : ICommand;
