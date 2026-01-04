namespace Accounting.Application.Reports.PeriodStatus.v1.Services;
public interface IPeriodStatusReportService { Task<byte[]> GenerateReportAsync(); }
