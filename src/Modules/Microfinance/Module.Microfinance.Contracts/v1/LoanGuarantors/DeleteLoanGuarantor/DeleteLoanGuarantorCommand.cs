using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanGuarantors.DeleteLoanGuarantor;

public sealed record DeleteLoanGuarantorCommand(Guid Id) : ICommand;
