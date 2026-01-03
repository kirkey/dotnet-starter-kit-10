namespace FSH.Modules.Microfinance.Features.v1.AgentBankings.CreateAgentBanking;

public class CreateAgentBankingValidator : AbstractValidator<CreateAgentBankingCommand>
{
    public CreateAgentBankingValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
