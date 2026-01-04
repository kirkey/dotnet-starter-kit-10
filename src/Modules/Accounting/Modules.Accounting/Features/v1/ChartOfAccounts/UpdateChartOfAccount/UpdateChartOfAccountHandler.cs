using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.ChartOfAccounts.UpdateChartOfAccount;

public record UpdateChartOfAccountCommand(
    Guid Id,
    string AccountCode,
    string AccountName,
    string AccountType,
    string UsoaCategory,
    Guid? ParentAccountId,
    string? ParentCode,
    decimal Balance,
    bool IsControlAccount,
    string NormalBalance,
    bool IsUsoaCompliant,
    string? RegulatoryClassification,
    string? Description,
    string? Notes) : ICommand<Guid>;

public class UpdateChartOfAccountHandler(AccountingDbContext context) : ICommandHandler<UpdateChartOfAccountCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateChartOfAccountCommand command, CancellationToken ct)
    {
        var entity = await context.ChartOfAccounts.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("ChartOfAccount not found");
        
        entity.Update(
            command.AccountCode,
            command.AccountName,
            command.AccountType,
            command.UsoaCategory,
            command.ParentAccountId,
            command.ParentCode,
            command.Balance,
            command.IsControlAccount,
            command.NormalBalance,
            command.IsUsoaCompliant,
            command.RegulatoryClassification,
            command.Description,
            command.Notes);
        
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
