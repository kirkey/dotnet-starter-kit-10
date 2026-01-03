namespace FSH.Modules.Microfinance.Features.v1.CreditBureauInquirys.CreateCreditBureauInquiry;

public class CreateCreditBureauInquiryValidator : AbstractValidator<CreateCreditBureauInquiryCommand>
{
    public CreateCreditBureauInquiryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
