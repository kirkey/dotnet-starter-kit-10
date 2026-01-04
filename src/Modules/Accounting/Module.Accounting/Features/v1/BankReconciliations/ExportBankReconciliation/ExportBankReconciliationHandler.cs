using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using Accounting.Application.Reports.BankReconciliation.v1.Services;
using Microsoft.EntityFrameworkCore;
using FSH.Module.Accounting.Contracts.v1.BankReconciliations;

using FSH.Module.Accounting.Contracts.v1.BankReconciliations.ExportBankReconciliation;

namespace FSH.Module.Accounting.Features.v1.BankReconciliations.ExportBankReconciliation;

/// <summary>
/// Query to export a Bank Reconciliation report for a specific reconciliation instance.
/// </summary>
/// <param name="Id">BankReconciliation Id to export</param>

/// <summary>
/// Handler for exporting bank reconciliation reports via IBankReconciliationReportService.
/// </summary>
public class ExportBankReconciliationHandler(AccountingDbContext context, IBankReconciliationReportService reportService) 
    : IQueryHandler<ExportBankReconciliationQuery, ExportBankReconciliationResult>
{
    public async ValueTask<ExportBankReconciliationResult> Handle(ExportBankReconciliationQuery query, CancellationToken ct)
    {
        var entity = await context.BankReconciliations.FirstOrDefaultAsync(x => x.Id == query.Id, ct)
            ?? throw new NotFoundException("BankReconciliation not found");

        var result = await reportService.GenerateReportAsync(query.Id);

        byte[] data;
        string contentType;
        string fileName = $"bankreconciliation_{query.Id}_{DateTime.UtcNow:yyyyMMddHHmmss}.{(query.Format?.ToLowerInvariant() == "csv" ? "csv" : "pdf")}";

        if (result is byte[] b)
        {
            data = b;
            contentType = "application/octet-stream";
        }
        else if (result is string s)
        {
            data = System.Text.Encoding.UTF8.GetBytes(s);
            contentType = "text/plain";
        }
        else if (result is null)
        {
            data = Array.Empty<byte>();
            contentType = "application/octet-stream";
        }
        else
        {
            var json = System.Text.Json.JsonSerializer.Serialize(result);
            data = System.Text.Encoding.UTF8.GetBytes(json);
            contentType = "application/json";
        }

        return new ExportBankReconciliationResult(data, contentType, fileName);
    }
}
