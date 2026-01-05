using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.FiscalPeriodClose.InitiateFiscalPeriodClose;

namespace FSH.Module.Accounting.Features.v1.FiscalPeriodClose.InitiateFiscalPeriodClose;

/// <summary>
/// Command to initiate the fiscal period close workflow for a given FiscalPeriodClose record.
/// </summary>
/// <param name="Id">FiscalPeriodClose ID to initiate</param> 

/// <summary>
/// Handler for initiating a fiscal period close (BeginClose) which locks transactional operations and prepares the period for final close.
/// </summary>
/// <remarks>
/// Responsibility: Transition FiscalPeriodClose aggregate to Initiated state (BeginClose), often performs checks for open transactions and emits domain events.
/// 
/// Execution Flow:
/// 1. Load FiscalPeriodClose aggregate
/// 2. Call BeginClose() domain method to set flags and perform pre-close validation
/// 3. Persist changes
/// 
/// Business Rules:
/// - Cannot initiate if period already completed
/// - Emit FiscalPeriodInitiated event for downstream processes
/// 
/// Permissions: Requires FiscalPeriodClose.Initiate
/// 
/// Exceptions:
/// - NotFoundException: Thrown if the fiscal period close record is not found
/// - BusinessRuleException: Thrown if pre-close validation fails
/// </remarks>
public class InitiateFiscalPeriodCloseHandler(AccountingDbContext context) 
    : ICommandHandler<InitiateFiscalPeriodCloseCommand>
{
    public async ValueTask<Unit> Handle(InitiateFiscalPeriodCloseCommand command, CancellationToken ct)
    {
        var entity = await context.FiscalPeriodClose.FindAsync(command.Id, ct) ?? throw new NotFoundException("FiscalPeriodClose not found");
        entity.BeginClose();
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
} 
