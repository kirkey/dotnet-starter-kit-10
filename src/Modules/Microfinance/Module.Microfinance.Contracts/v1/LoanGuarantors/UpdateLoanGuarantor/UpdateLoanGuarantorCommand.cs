using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanGuarantors.UpdateLoanGuarantor;

public sealed record UpdateLoanGuarantorCommand(Guid Id, string Name) : ICommand<Guid>;
