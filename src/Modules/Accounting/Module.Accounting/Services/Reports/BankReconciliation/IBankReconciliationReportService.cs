using System.Threading.Tasks;

namespace Accounting.Application.Reports.BankReconciliation.v1.Services;

public interface IBankReconciliationReportService
{
    Task<object?> GenerateReportAsync(Guid id);
}