using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.TaxCodes.GetListTaxCode;

public sealed record GetTaxCodesQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<TaxCodesPagedResponse>;

public sealed record TaxCodesPagedResponse(
    List<TaxCodeSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);