namespace Accounting.Application.Reports.FiscalPeriodClose.v1.Services;
public interface IFiscalPeriodCloseReportService { Task<byte[]> GenerateReportAsync(); }
