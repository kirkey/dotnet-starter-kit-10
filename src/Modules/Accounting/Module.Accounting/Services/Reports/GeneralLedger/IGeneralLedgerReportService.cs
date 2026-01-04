using System.Threading.Tasks;

namespace Accounting.Application.Reports.GeneralLedger.v1.Services;

public interface IGeneralLedgerReportService
{
    Task<object?> GenerateReportAsync(DateTime from, DateTime to, Guid? entityId = null);
}