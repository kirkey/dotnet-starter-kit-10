using Mediator;
using FSH.Module.Accounting.Contracts.v1.DepreciationMethods;

namespace FSH.Module.Accounting.Contracts.v1.DepreciationMethods.GetListDepreciationMethod;

public sealed record GetDepreciationMethodsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<DepreciationMethodsPagedResponse>;

public sealed record DepreciationMethodsPagedResponse(
    List<DepreciationMethodSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);
