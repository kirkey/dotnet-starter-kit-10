using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.PaymentGateways.UpdatePaymentGateway;

public sealed record UpdatePaymentGatewayCommand(Guid Id, string Name) : ICommand<Guid>;
