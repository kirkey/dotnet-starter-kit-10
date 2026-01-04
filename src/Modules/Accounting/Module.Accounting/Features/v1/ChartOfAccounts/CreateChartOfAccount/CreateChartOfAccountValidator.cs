using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.ChartOfAccounts.CreateChartOfAccount;

namespace FSH.Module.Accounting.Features.v1.ChartOfAccounts.CreateChartOfAccount;

public class CreateChartOfAccountValidator : AbstractValidator<CreateChartOfAccountCommand>
{
    public CreateChartOfAccountValidator()
    {
        RuleFor(x => x.AccountCode)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.AccountCode)
            .WithMessage("Account code is required and must not exceed {MaxLength} characters.");
            
        RuleFor(x => x.AccountName)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.AccountName)
            .WithMessage("Account name is required and must not exceed {MaxLength} characters.");
            
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
            
        When(x => !string.IsNullOrEmpty(x.Description), () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(AccountingStringLengths.Description);
        });
            
        When(x => !string.IsNullOrEmpty(x.Notes), () =>
        {
            RuleFor(x => x.Notes)
                .MaximumLength(AccountingStringLengths.Notes);
        });
    }
    
    private static bool BeValidAccountType(string accountType)
    {
        var validTypes = new[] { "Asset", "Liability", "Equity", "Revenue", "Expense" };
        return validTypes.Contains(accountType, StringComparer.OrdinalIgnoreCase);
    }
}
