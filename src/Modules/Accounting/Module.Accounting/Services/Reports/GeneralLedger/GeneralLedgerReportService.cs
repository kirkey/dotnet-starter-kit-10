using System.Threading.Tasks;

namespace Accounting.Application.Reports.GeneralLedger.v1.Services;

internal class GeneralLedgerReportService : IGeneralLedgerReportService
{
    public Task<object?> GenerateReportAsync(DateTime from, DateTime to, Guid? entityId = null) => Task.FromResult<object?>(null);
}