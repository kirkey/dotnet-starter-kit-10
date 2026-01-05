using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CreditBureauInquirys.GetCreditBureauInquiry;

public sealed record GetCreditBureauInquiryQuery(Guid Id) : IQuery<CreditBureauInquiryDto>;
