using System.Threading.Tasks;

namespace Accounting.Application.Reports.TrialBalance.v1.Services;

internal class TrialBalanceReportService : ITrialBalanceReportService
{
    public Task<object?> GenerateReportAsync(DateTime asOf, object? options) => Task.FromResult<object?>(null);
}