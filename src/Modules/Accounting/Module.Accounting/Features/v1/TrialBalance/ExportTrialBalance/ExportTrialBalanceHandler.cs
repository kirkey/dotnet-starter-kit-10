using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using Accounting.Application.Reports.TrialBalance.v1.Services;
using Microsoft.EntityFrameworkCore;
using FSH.Module.Accounting.Contracts.v1.TrialBalance.ExportTrialBalance;

namespace FSH.Module.Accounting.Features.v1.TrialBalance.ExportTrialBalance;
/// <summary>
/// Handler for exporting Trial Balance reports using the trial balance report service.
/// </summary>
/// <remarks>
/// Responsibility: Validate TrialBalance entity, generate report using report service, and return exported bytes.
/// </remarks>
public class ExportTrialBalanceHandler(AccountingDbContext context, ITrialBalanceReportService reportService) 
    : IQueryHandler<ExportTrialBalanceQuery, ExportTrialBalanceResult>
{
    public async ValueTask<ExportTrialBalanceResult> Handle(ExportTrialBalanceQuery query, CancellationToken ct)
    {
        var entity = await context.TrialBalance.FirstOrDefaultAsync(x => x.Id == query.Id, ct)
            ?? throw new NotFoundException("TrialBalance not found");

        var result = await reportService.GenerateReportAsync(DateTime.UtcNow, null);

        byte[] data;
        string contentType;
        string fileName = $"trialbalance_{query.Id}_{DateTime.UtcNow:yyyyMMddHHmmss}.{(query.Format?.ToLowerInvariant() == "csv" ? "csv" : "pdf")}";

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

        return new ExportTrialBalanceResult(data, contentType, fileName);
    }
} 
