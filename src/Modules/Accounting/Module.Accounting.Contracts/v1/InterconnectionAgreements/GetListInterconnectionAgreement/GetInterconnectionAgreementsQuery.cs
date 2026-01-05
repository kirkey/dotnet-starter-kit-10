using Mediator;
using FSH.Module.Accounting.Contracts.v1.InterconnectionAgreements;

namespace FSH.Module.Accounting.Contracts.v1.InterconnectionAgreements.GetListInterconnectionAgreement;

public sealed record GetInterconnectionAgreementsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<InterconnectionAgreementsPagedResponse>;

public sealed record InterconnectionAgreementsPagedResponse(
    List<InterconnectionAgreementSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);
