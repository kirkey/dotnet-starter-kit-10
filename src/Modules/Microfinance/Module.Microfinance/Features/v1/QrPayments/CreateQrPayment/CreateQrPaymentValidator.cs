namespace FSH.Module.Microfinance.Features.v1.QrPayments.CreateQrPayment;

public class CreateQrPaymentValidator : AbstractValidator<CreateQrPaymentCommand>
{
    public CreateQrPaymentValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
