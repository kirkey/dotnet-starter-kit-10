namespace FSH.Modules.Microfinance.Features.v1.FeePayments.CreateFeePayment;

public class CreateFeePaymentValidator : AbstractValidator<CreateFeePaymentCommand>
{
    public CreateFeePaymentValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
