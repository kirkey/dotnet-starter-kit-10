using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.ChartOfAccounts.CreateChartOfAccount;
using FSH.Module.Accounting.Features;

namespace FSH.Module.Accounting.Features.v1.ChartOfAccounts.CreateChartOfAccount;

public class CreateChartOfAccountValidator : AbstractValidator<CreateChartOfAccountCommand>
{
    public CreateChartOfAccountValidator()
    {
        RuleFor(x => x.AccountCode)
            .ValidateAccountCode();
            
        RuleFor(x => x.AccountName)
            .ValidateAccountName();
            
        RuleFor(x => x.AccountType)
            .NotEmpty()
            .Must(BeValidAccountType)
            .WithMessage("Account type must be Asset, Liability, Equity, Revenue, or Expense.");
            
        RuleFor(x => x.UsoaCategory)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.UsoaCategory)
            .WithMessage("USOA category is required and must not exceed {MaxLength} characters.");
            
        RuleFor(x => x.NormalBalance)
            .NotEmpty()
            .Must(x => x == "Debit" || x == "Credit")
            .WithMessage("Normal balance must be either Debit or Credit.");
            
        When(x => !string.IsNullOrEmpty(x.ParentCode), () =>
        {
            RuleFor(x => x.ParentCode)
                .MaximumLength(AccountingStringLengths.Regular);
        });
            
        When(x => !string.IsNullOrEmpty(x.RegulatoryClassification), () =>
        {
            RuleFor(x => x.RegulatoryClassification)
                .MaximumLength(AccountingStringLengths.RegulatoryClassification);
        });
            
        RuleFor(x => x.Description)
            .ValidateDescription();
            
        RuleFor(x => x.Notes)
            .ValidateNotes();
    }
    
    private static bool BeValidAccountType(string accountType)
    {
        var validTypes = new[] { "Asset", "Liability", "Equity", "Revenue", "Expense" };
        return validTypes.Contains(accountType, StringComparer.OrdinalIgnoreCase);
    }
}
