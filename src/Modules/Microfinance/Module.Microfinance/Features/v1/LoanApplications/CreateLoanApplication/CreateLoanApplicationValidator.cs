using FSH.Module.Microfinance.Contracts.v1.LoanApplications;

namespace FSH.Module.Microfinance.Features.v1.LoanApplications.CreateLoanApplication;

/// <summary>
/// Validator for CreateLoanApplicationCommand.
/// 
/// **Validation Rules:**
/// - Name: Required, max 256 chars
/// </summary>
public class CreateLoanApplicationValidator : AbstractValidator<CreateLoanApplicationCommand>
{
    public CreateLoanApplicationValidator()
    {
        RuleFor(x => x.Name).ValidateName();
    }
}
