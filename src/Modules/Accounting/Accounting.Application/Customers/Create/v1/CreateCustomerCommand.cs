namespace Accounting.Application.Customers.Create.v1;

/// <summary>
/// Command to create a new customer account.
/// </summary>
public record CreateCustomerCommand(
    string CustomerNumber,
    string CustomerName,
    string CustomerType,
    string BillingAddress,
    string? ShippingAddress,
    string? Email,
    string? Phone,
    string? ContactName,
    decimal CreditLimit,
    string PaymentTerms,
    bool TaxExempt,
    string? TaxId,
    decimal DiscountPercentage,
    DefaultIdType? DefaultRateScheduleId,
    DefaultIdType? ReceivableAccountId,
    string? SalesRepresentative,
    string? Description,
    string? Notes
) : IRequest<CustomerCreateResponse>;

