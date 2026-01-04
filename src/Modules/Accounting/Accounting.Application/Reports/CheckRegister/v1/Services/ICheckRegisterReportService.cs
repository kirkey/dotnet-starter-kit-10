namespace Accounting.Application.Reports.CheckRegister.v1.Services;

public interface ICheckRegisterReportService
{
    Task<byte[]> GenerateReportAsync(DateTime startDate, DateTime endDate);
}
