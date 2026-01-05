using FSH.Module.Microfinance.Contracts.v1.CreditBureauInquirys.CreateCreditBureauInquiry;

namespace FSH.Module.Microfinance.Features.v1.CreditBureauInquirys.CreateCreditBureauInquiry;

public class CreateCreditBureauInquiryValidator : AbstractValidator<CreateCreditBureauInquiryCommand>
{
    public CreateCreditBureauInquiryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
