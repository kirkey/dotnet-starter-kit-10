using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Banks;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Banks.GetBank;

/// <summary>
/// Query to retrieve a single bank account by ID.
/// </summary>
/// <param name="Id">Bank ID (Guid) to retrieve</param>
public record GetBankQuery(Guid Id) : IQuery<BankDto>;

/// <summary>
/// Handler for retrieving a single bank account by ID with complete DTO projection.
/// </summary>
/// <remarks>
/// Responsibility: Execute the bank query and return a DTO with 15 returned fields.
/// 
/// Execution Flow:
/// 1. Query Banks DbSet by Id using Where(x => x.Id == query.Id)
/// 2. Project to BankDto with 15 fields: Id, BankName, BankCode, Address, ContactName, ContactPhone, 
///    RoutingNumber, SwiftCode, CurrencyCode, OpeningBalance, CurrentBalance, IsDefault, Description, 
///    IsActive, CreatedOnUtc
/// 3. Execute FirstOrDefaultAsync() to retrieve single result
/// 4. Throw NotFoundException if entity not found
/// 
/// Returned Fields (BankDto): Id, BankName, BankCode, Address, ContactName, ContactPhone, 
/// RoutingNumber, SwiftCode, CurrencyCode, OpeningBalance, CurrentBalance, IsDefault, Description, 
/// IsActive, CreatedOnUtc
/// 
/// Permissions: Requires authenticated user (any authorized role)
/// 
/// Exceptions:
/// - NotFoundException: Thrown if bank with specified ID not found
/// </remarks>
public class GetBankHandler(AccountingDbContext context) : IQueryHandler<GetBankQuery, BankDto>
{
    public async ValueTask<BankDto> Handle(GetBankQuery query, CancellationToken ct)
    {
        var entity = await context.Banks
            .Where(x => x.Id == query.Id)
            .Select(x => new BankDto(
                x.Id,
                x.BankName,
                x.BankCode,
                x.Address,
                x.ContactName,
                x.ContactPhone,
                x.RoutingNumber,
                x.SwiftCode,
                x.CurrencyCode,
                x.OpeningBalance,
                x.CurrentBalance,
                x.IsDefault,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("Bank not found");
    }
}
