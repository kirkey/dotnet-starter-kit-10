namespace Accounting.Application.Reports.WriteOffSummary.v1.Services;
public interface IWriteOffSummaryReportService { Task<byte[]> GenerateReportAsync(); }
