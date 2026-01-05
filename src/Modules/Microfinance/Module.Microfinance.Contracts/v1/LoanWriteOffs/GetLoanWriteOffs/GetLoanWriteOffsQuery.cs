using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanWriteOffs.GetLoanWriteOffs;

public sealed record GetLoanWriteOffsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<LoanWriteOffsPagedResponse>;

public sealed record LoanWriteOffsPagedResponse(List<LoanWriteOffSummaryDto> Items, int TotalCount, int Page, int PageSize);
