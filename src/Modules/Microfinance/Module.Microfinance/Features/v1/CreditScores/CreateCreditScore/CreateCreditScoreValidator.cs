using FSH.Module.Microfinance.Contracts.v1.CreditScores.CreateCreditScore;

namespace FSH.Module.Microfinance.Features.v1.CreditScores.CreateCreditScore;

public class CreateCreditScoreValidator : AbstractValidator<CreateCreditScoreCommand>
{
    public CreateCreditScoreValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
