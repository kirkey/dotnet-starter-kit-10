namespace Accounting.Application.Reports.PrepaidExpense.v1.Services;
public interface IPrepaidExpenseReportService { Task<byte[]> GenerateReportAsync(); }
