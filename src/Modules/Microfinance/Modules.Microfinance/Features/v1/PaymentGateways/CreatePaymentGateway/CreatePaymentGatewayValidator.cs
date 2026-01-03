namespace FSH.Modules.Microfinance.Features.v1.PaymentGateways.CreatePaymentGateway;

public class CreatePaymentGatewayValidator : AbstractValidator<CreatePaymentGatewayCommand>
{
    public CreatePaymentGatewayValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
