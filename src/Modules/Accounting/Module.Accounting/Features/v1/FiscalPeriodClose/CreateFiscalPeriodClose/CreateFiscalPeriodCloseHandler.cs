using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.FiscalPeriodClose.CreateFiscalPeriodClose;

/// <summary>
/// Command to create a FiscalPeriodClose entry representing a period to be closed.
/// </summary>
/// <param name="FiscalPeriodId">Reference to the fiscal period to close (required)</param>
/// <param name="FiscalYear">Fiscal year number</param>
/// <param name="PeriodName">Descriptive name for the period (e.g., "Jan 2025")</param>
/// <param name="StartDate">Period start date</param>
/// <param name="EndDate">Period end date</param>
/// <param name="RetainedEarnings">Optional retained earnings adjustment</param>
/// <param name="Description">Optional descriptive notes</param>
public record CreateFiscalPeriodCloseCommand(
    Guid FiscalPeriodId,
    int FiscalYear,
    string PeriodName,
    DateTime StartDate,
    DateTime EndDate,
    decimal RetainedEarnings = 0,
    string? Description = null) : ICommand<Guid>;

/// <summary>
/// Handler for creating a FiscalPeriodClose aggregate that will be used to manage period close workflows.
/// </summary>
/// <remarks>
/// Responsibility: Instantiate a FiscalPeriodClose using domain factory and record creator audit fields.
/// 
/// Execution Flow:
/// 1. Use FiscalPeriodClose.Create(...) to build the aggregate with tenant and current user context
/// 2. Add to DbSet and SaveChangesAsync
/// 3. Return created FiscalPeriodClose ID
/// 
/// Notes:
/// - RetainedEarnings is used to create closing GL entries during completion
/// - Period start/end dates are used to lock transactions during close
/// 
/// Permissions: Requires authenticated user with FiscalPeriodClose.Create permission
/// 
/// Exceptions:
/// - DbException: Thrown if referenced fiscal period not found
/// - ValidationException: Thrown if dates are invalid
/// </remarks>
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
