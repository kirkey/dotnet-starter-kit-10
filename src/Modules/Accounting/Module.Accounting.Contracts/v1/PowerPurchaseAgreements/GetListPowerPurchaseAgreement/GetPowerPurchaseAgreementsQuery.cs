using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PowerPurchaseAgreements.GetListPowerPurchaseAgreement;

public sealed record GetPowerPurchaseAgreementsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<PowerPurchaseAgreementsPagedResponse>;

public sealed record PowerPurchaseAgreementsPagedResponse(
    List<PowerPurchaseAgreementSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);