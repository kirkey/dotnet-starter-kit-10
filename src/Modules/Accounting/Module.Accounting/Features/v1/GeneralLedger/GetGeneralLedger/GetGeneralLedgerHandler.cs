using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.GeneralLedger;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.GeneralLedger.GetGeneralLedger;

/// <summary>
/// Query to retrieve a single General Ledger account definition by ID.
/// </summary>
/// <param name="Id">General Ledger account ID to retrieve</param>

/// <summary>
/// Handler for retrieving a single GeneralLedger DTO with core metadata.
/// </summary>
/// <remarks>
/// Responsibility: Project GeneralLedger aggregate to DTO including Id, Name, Description, IsActive, CreatedOnUtc.
/// 
/// Use Cases: GL account configuration review and mapping
/// 
/// Permissions: Requires GeneralLedger.View
/// 
/// Exceptions:
/// - NotFoundException: Thrown if GL account not found
/// </remarks>
public class GetGeneralLedgerHandler(AccountingDbContext context) : IQueryHandler<GetGeneralLedgerQuery, GeneralLedgerDto>
{
    public async ValueTask<GeneralLedgerDto> Handle(GetGeneralLedgerQuery query, CancellationToken ct)
    {
        var entity = await context.GeneralLedger
            .Where(x => x.Id == query.Id)
            .Select(x => new GeneralLedgerDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("GeneralLedger not found");
    }
} 
