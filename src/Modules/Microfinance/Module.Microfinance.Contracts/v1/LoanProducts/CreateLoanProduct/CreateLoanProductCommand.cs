using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanProducts.CreateLoanProduct;

public sealed record CreateLoanProductCommand(string Name) : ICommand<Guid>;
