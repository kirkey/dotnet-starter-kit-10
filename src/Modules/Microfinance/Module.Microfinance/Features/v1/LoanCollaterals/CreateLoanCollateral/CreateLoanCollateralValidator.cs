using FSH.Module.Microfinance.Contracts.v1.LoanCollaterals.CreateLoanCollateral;

namespace FSH.Module.Microfinance.Features.v1.LoanCollaterals.CreateLoanCollateral;

public class CreateLoanCollateralValidator : AbstractValidator<CreateLoanCollateralCommand>
{
    public CreateLoanCollateralValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
