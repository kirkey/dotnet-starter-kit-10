using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CreditBureauInquirys.CreateCreditBureauInquiry;

public sealed record CreateCreditBureauInquiryCommand(string Name) : ICommand<Guid>;
