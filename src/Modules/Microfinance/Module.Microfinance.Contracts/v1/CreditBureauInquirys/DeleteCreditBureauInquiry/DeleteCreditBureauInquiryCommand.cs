using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CreditBureauInquirys.DeleteCreditBureauInquiry;

public sealed record DeleteCreditBureauInquiryCommand(Guid Id) : ICommand;
