using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.PromiseToPays.GetPromiseToPays;

public sealed record GetPromiseToPaysQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<PromiseToPaysPagedResponse>;
