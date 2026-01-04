namespace Accounting.Application.Reports.BudgetVsActual.v1.Services;
public interface IBudgetVsActualReportService { Task<byte[]> GenerateReportAsync(); }
