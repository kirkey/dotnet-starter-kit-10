using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PatronageCapital.GetListPatronageCapital;

public record GetPatronageCapitalQuery(int Page = 1, int PageSize = 10, string? SearchTerm = null, bool? IsActive = null) : IQuery<PatronageCapitalPagedResponse>;

public record PatronageCapitalPagedResponse(List<PatronageCapitalSummaryDto> Items, int TotalCount, int Page, int PageSize);