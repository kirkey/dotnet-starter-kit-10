using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.FiscalPeriodClose.CreateFiscalPeriodClose;

public record CreateFiscalPeriodCloseCommand(
    Guid FiscalPeriodId,
    int FiscalYear,
    string PeriodName,
    DateTime StartDate,
    DateTime EndDate,
    decimal RetainedEarnings = 0,
    string? Description = null) : ICommand<Guid>;

public class CreateFiscalPeriodCloseHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateFiscalPeriodCloseCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateFiscalPeriodCloseCommand command, CancellationToken ct)
    {
        var entity = FiscalPeriodClose.Create(
            command.FiscalPeriodId,
            command.FiscalYear,
            command.PeriodName,
            command.StartDate,
            command.EndDate,
            command.RetainedEarnings,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.FiscalPeriodClose.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
