namespace Accounting.Application.Reports.FixedAssetRegister.v1.Services;
public interface IFixedAssetRegisterReportService { Task<byte[]> GenerateReportAsync(); }
