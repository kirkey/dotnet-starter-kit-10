using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanProducts.GetLoanProduct;

public sealed record GetLoanProductQuery(Guid Id) : IQuery<LoanProductDto>;
