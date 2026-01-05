using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Payments;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

using FSH.Module.Accounting.Contracts.v1.Payments.GetPayment;namespace FSH.Module.Accounting.Features.v1.Payments.GetPayment;

/// <summary>
/// Handler for retrieving a single Payment by ID.
/// 
/// **Responsibility:**
/// Queries the database for a Payment by ID and returns it as a DTO.
/// 
/// **Execution Flow:**
/// 1. Query DbSet for Payment by ID
/// 2. Project to PaymentDto with all payment properties
/// 3. Throw NotFoundException if not found
/// 4. Return the DTO
/// 
/// **Returned Fields:**
/// - Id: Unique identifier
/// - Name: Payment identifier/name
/// - Description: Optional description
/// - IsActive: Active status
/// - CreatedOnUtc: Creation date
/// 
/// **Permissions:**
/// Requires: Accounting.Payment.View
/// 
/// **Exceptions:**
/// - NotFoundException: Thrown when Payment is not found
/// </summary>
public class GetPaymentHandler(AccountingDbContext context) : IQueryHandler<GetPaymentQuery, PaymentDto>
{
    /// <summary>
    /// Handles the GetPaymentQuery to retrieve a Payment.
    /// </summary>
    /// <param name="query">The query containing the Payment ID to retrieve</param>
    /// <param name="ct">Cancellation token for the operation</param>
    /// <returns>The PaymentDto with payment details</returns>
    /// <exception cref="NotFoundException">Thrown when Payment with the specified ID is not found</exception>
    public async ValueTask<PaymentDto> Handle(GetPaymentQuery query, CancellationToken ct)
    {
        var entity = await context.Payments
            .Where(x => x.Id == query.Id)
            .Select(x => new PaymentDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("Payment not found");
    }
}
