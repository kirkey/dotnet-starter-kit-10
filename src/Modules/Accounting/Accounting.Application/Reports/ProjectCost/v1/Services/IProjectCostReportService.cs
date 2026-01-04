namespace Accounting.Application.Reports.ProjectCost.v1.Services;
public interface IProjectCostReportService { Task<byte[]> GenerateReportAsync(); }
