using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CreditBureauInquirys.UpdateCreditBureauInquiry;

public sealed record UpdateCreditBureauInquiryCommand(Guid Id, string Name) : ICommand<Guid>;
