namespace Accounting.Application.Reports.DepreciationSchedule.v1.Services;
public interface IDepreciationScheduleReportService { Task<byte[]> GenerateReportAsync(); }
