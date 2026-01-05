using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanProducts.UpdateLoanProduct;

public sealed record UpdateLoanProductCommand(Guid Id, string Name) : ICommand<Guid>;
