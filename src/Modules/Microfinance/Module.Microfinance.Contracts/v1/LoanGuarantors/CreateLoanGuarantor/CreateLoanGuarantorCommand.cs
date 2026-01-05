using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanGuarantors.CreateLoanGuarantor;

public sealed record CreateLoanGuarantorCommand(string Name) : ICommand<Guid>;
