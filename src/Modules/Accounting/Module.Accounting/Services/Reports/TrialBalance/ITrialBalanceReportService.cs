using System.Threading.Tasks;

namespace Accounting.Application.Reports.TrialBalance.v1.Services;

public interface ITrialBalanceReportService
{
    Task<object?> GenerateReportAsync(DateTime asOf, object? options);
}