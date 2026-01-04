namespace Accounting.Application.Reports.DeferredRevenue.v1.Services;
public interface IDeferredRevenueReportService { Task<byte[]> GenerateReportAsync(); }
