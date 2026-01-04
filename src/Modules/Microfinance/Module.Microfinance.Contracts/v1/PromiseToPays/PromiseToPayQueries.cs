namespace FSH.Module.Microfinance.Contracts.v1.PromiseToPays;

public record GetPromiseToPayQuery(Guid Id);
public record GetPromiseToPaysQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record PromiseToPaysPagedResponse(List<PromiseToPaySummaryDto> Items, int TotalCount, int Page, int PageSize);
