using FSH.Framework.Web.ImportExport;
using FSH.Module.Todos.Contracts.v1.Todos;
using FSH.Module.Todos.Data;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Todos.Features.v1.Todos.ExportTodos;

public sealed class ExportTodosQueryHandler(TodoDbContext context) : IQueryHandler<ExportTodosQuery, ExportTodosResult>
{
    public async ValueTask<ExportTodosResult> Handle(ExportTodosQuery query, CancellationToken cancellationToken)
    {
        var todos = await context.Todos
            .AsNoTracking()
            .OrderBy(t => t.CreatedOnUtc)
            .Select(t => new TodoExportDto(
                t.Name,
                t.Description,
                t.Notes,
                t.Status.ToString(),
                (int)t.Priority,
                t.DueDate,
                t.IsCompleted,
                t.CompletedAt))
            .ToListAsync(cancellationToken);

        IDataExporter<TodoExportDto> exporter = query.Format.ToLowerInvariant() switch
        {
            "json" => new JsonDataExporter<TodoExportDto>(),
            _ => new CsvDataExporter<TodoExportDto>()
        };

        var result = await exporter.ExportAsync(todos, $"todos_{DateTime.UtcNow:yyyyMMdd}", cancellationToken);

        return new ExportTodosResult(result.Data, result.ContentType, result.FileName);
    }
}
