using FSH.Module.Microfinance.Contracts.v1.SavingsAccounts;

namespace FSH.Module.Microfinance.Features.v1.SavingsAccounts.CreateSavingsAccount;

/// <summary>
/// Validator for CreateSavingsAccountCommand.
/// 
/// **Validation Rules:**
/// - Name: Required, max 256 chars
/// </summary>
public class CreateSavingsAccountValidator : AbstractValidator<CreateSavingsAccountCommand>
{
    public CreateSavingsAccountValidator()
    {
        RuleFor(x => x.Name).ValidateName();
    }
}
