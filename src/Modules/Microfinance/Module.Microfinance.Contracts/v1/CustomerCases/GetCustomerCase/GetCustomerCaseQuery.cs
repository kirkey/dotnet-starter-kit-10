using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CustomerCases.GetCustomerCase;

public sealed record GetCustomerCaseQuery(Guid Id) : IQuery<CustomerCaseDto>;
