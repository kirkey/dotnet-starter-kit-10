using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.ChartOfAccounts.CreateChartOfAccount;

public record CreateChartOfAccountCommand(
    string AccountCode,
    string AccountName,
    string AccountType,
    string UsoaCategory,
    Guid? ParentAccountId = null,
    string? ParentCode = null,
    decimal Balance = 0,
    bool IsControlAccount = false,
    string NormalBalance = "Debit",
    bool IsUsoaCompliant = true,
    string? RegulatoryClassification = null,
    string? Description = null,
    string? Notes = null) : ICommand<Guid>;

public class CreateChartOfAccountHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateChartOfAccountCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateChartOfAccountCommand command, CancellationToken ct)
    {
        var entity = ChartOfAccount.Create(
            command.AccountCode,
            command.AccountName,
            command.AccountType,
            command.UsoaCategory,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.ParentAccountId,
            command.ParentCode,
            command.Balance,
            command.IsControlAccount,
            command.NormalBalance,
            command.IsUsoaCompliant,
            command.RegulatoryClassification,
            command.Description,
            command.Notes);
        
        context.ChartOfAccounts.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
