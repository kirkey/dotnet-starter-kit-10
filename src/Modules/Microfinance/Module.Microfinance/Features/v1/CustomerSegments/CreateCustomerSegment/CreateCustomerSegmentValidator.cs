namespace FSH.Module.Microfinance.Features.v1.CustomerSegments.CreateCustomerSegment;

public class CreateCustomerSegmentValidator : AbstractValidator<CreateCustomerSegmentCommand>
{
    public CreateCustomerSegmentValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
