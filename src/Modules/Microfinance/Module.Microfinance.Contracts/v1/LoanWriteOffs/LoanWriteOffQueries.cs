namespace FSH.Module.Microfinance.Contracts.v1.LoanWriteOffs;

public record GetLoanWriteOffQuery(Guid Id);
public record GetLoanWriteOffsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record LoanWriteOffsPagedResponse(List<LoanWriteOffSummaryDto> Items, int TotalCount, int Page, int PageSize);
