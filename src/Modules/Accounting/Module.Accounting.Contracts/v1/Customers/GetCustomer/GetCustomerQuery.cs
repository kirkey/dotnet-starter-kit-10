using FSH.Module.Accounting.Contracts.v1.Customers;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Customers.GetCustomer;

/// <summary>
/// Query to retrieve a single customer by ID.
/// </summary>
/// <param name="Id">Customer ID (Guid) to retrieve</param>
public record GetCustomerQuery(Guid Id) : IQuery<CustomerDto>;