using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanProducts.DeleteLoanProduct;

public sealed record DeleteLoanProductCommand(Guid Id) : ICommand;
