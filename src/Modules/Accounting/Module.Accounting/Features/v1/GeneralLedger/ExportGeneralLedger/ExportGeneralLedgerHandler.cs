using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using Accounting.Application.Reports.GeneralLedger.v1.Services;
using Microsoft.EntityFrameworkCore;
using FSH.Module.Accounting.Contracts.v1.GeneralLedger;

namespace FSH.Module.Accounting.Features.v1.GeneralLedger.ExportGeneralLedger;

/// <summary>
/// Query to export the General Ledger report for a specific GL account (full history or by date range).
/// </summary>
/// <param name="Id">GeneralLedger account Id to export</param>
public record ExportGeneralLedgerQuery(Guid Id, string Format = "pdf") : IQuery<ExportGeneralLedgerResult>;

/// <summary>
/// Handler for exporting General Ledger data using the IGeneralLedgerReportService.
/// </summary>
/// <remarks>
/// Responsibility: Validate GL account existence, invoke the report service to generate a ledger report, and return the exported bytes as an Export result.
/// </remarks>
public class ExportGeneralLedgerHandler(AccountingDbContext context, IGeneralLedgerReportService reportService) 
    : IQueryHandler<ExportGeneralLedgerQuery, ExportGeneralLedgerResult>
{
    public async ValueTask<ExportGeneralLedgerResult> Handle(ExportGeneralLedgerQuery query, CancellationToken ct)
    {
        var entity = await context.GeneralLedger.FirstOrDefaultAsync(x => x.Id == query.Id, ct)
            ?? throw new NotFoundException("GeneralLedger not found");

        var result = await reportService.GenerateReportAsync(DateTime.MinValue, DateTime.UtcNow, query.Id);

        // Normalize result to bytes/content type/file name
        byte[] data;
        string contentType;
        string fileName = $"generalledger_{query.Id}_{DateTime.UtcNow:yyyyMMddHHmmss}.{(query.Format?.ToLowerInvariant() == "csv" ? "csv" : "pdf")}";

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
            // Nothing generated - return empty file with TODO note
            data = Array.Empty<byte>();
            contentType = "application/octet-stream";
        }
        else
        {
            // Fallback: serialize to JSON
            var json = System.Text.Json.JsonSerializer.Serialize(result);
            data = System.Text.Encoding.UTF8.GetBytes(json);
            contentType = "application/json";
        }

        return new ExportGeneralLedgerResult(data, contentType, fileName);
    }
} 
