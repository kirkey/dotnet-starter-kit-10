using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.ChartOfAccounts.UpdateChartOfAccount;

public class UpdateChartOfAccountValidator : AbstractValidator<UpdateChartOfAccountCommand>
{
    public UpdateChartOfAccountValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        
        RuleFor(x => x.AccountCode)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.AccountCode);
            
        RuleFor(x => x.AccountName)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.AccountName);
            
        RuleFor(x => x.AccountType)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.Medium)
            .Must(BeValidAccountType).WithMessage("AccountType must be Asset, Liability, Equity, Revenue, or Expense");
            
        RuleFor(x => x.UsoaCategory)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.Medium);
            
        RuleFor(x => x.NormalBalance)
            .NotEmpty()
            .Must(x => x == "Debit" || x == "Credit").WithMessage("NormalBalance must be Debit or Credit");
            
        RuleFor(x => x.Balance)
            .NotNull();
            
        When(x => !string.IsNullOrEmpty(x.ParentCode), () =>
        {
            RuleFor(x => x.ParentCode)
                .MaximumLength(AccountingStringLengths.AccountCode);
        });
            
        When(x => !string.IsNullOrEmpty(x.RegulatoryClassification), () =>
        {
            RuleFor(x => x.RegulatoryClassification)
                .MaximumLength(AccountingStringLengths.Large);
        });
            
        When(x => !string.IsNullOrEmpty(x.Description), () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(AccountingStringLengths.Description);
        });
            
        When(x => !string.IsNullOrEmpty(x.Notes), () =>
        {
            RuleFor(x => x.Notes)
                .MaximumLength(AccountingStringLengths.XHuge);
        });
    }
    
    private bool BeValidAccountType(string accountType)
    {
        var validTypes = new[] { "Asset", "Liability", "Equity", "Revenue", "Expense" };
        return validTypes.Contains(accountType);
    }
}
