namespace Accounting.Application.Reports.VarianceAnalysis.v1.Services;
public interface IVarianceAnalysisReportService { Task<byte[]> GenerateReportAsync(); }
