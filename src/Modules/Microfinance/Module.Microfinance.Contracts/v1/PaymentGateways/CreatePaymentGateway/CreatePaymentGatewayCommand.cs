using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.PaymentGateways.CreatePaymentGateway;

public sealed record CreatePaymentGatewayCommand(string Name) : ICommand<Guid>;
