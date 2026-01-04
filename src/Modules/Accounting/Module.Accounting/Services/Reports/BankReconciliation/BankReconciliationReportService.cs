using System.Threading.Tasks;

namespace Accounting.Application.Reports.BankReconciliation.v1.Services;

internal class BankReconciliationReportService : IBankReconciliationReportService
{
    public Task<object?> GenerateReportAsync(Guid id) => Task.FromResult<object?>(null);
}