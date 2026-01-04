namespace FSH.Module.Microfinance.Features.v1.LoanWriteOffs.CreateLoanWriteOff;

public class CreateLoanWriteOffValidator : AbstractValidator<CreateLoanWriteOffCommand>
{
    public CreateLoanWriteOffValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
