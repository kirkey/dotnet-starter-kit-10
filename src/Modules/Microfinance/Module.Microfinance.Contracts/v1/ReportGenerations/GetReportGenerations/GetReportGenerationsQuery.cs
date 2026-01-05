using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ReportGenerations.GetReportGenerations;

public sealed record GetReportGenerationsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<ReportGenerationsPagedResponse>;
