using FSH.Framework.Core.Context;
using FSH.Framework.Web.ImportExport;
using FSH.Module.Todos.Contracts.v1.Todos;
using FSH.Module.Todos.Data;
using FSH.Module.Todos.Domain;

namespace FSH.Module.Todos.Features.v1.Todos.ImportTodos;

public sealed class ImportTodosCommandHandler(TodoDbContext context, ICurrentUser currentUser) : ICommandHandler<ImportTodosCommand, ImportTodosResult>
{
    public async ValueTask<ImportTodosResult> Handle(ImportTodosCommand command, CancellationToken cancellationToken)
    {
        IDataImporter<TodoImportDto> importer;

        if (command.ContentType.Contains("json", StringComparison.OrdinalIgnoreCase))
        {
            importer = new JsonDataImporter<TodoImportDto>();
        }
        else if (command.ContentType.Contains("csv", StringComparison.OrdinalIgnoreCase))
        {
            importer = new CsvDataImporter<TodoImportDto>();
        }
        else
        {
            return new ImportTodosResult(0, 0, new[] { "Unsupported file format. Please use CSV or JSON." });
        }

        var importResult = await importer.ImportAsync(command.FileStream, cancellationToken);

        if (importResult.Data.Count == 0)
        {
            return new ImportTodosResult(0, 0, importResult.Errors);
        }

        var todos = importResult.Data.Select(dto => Domain.Todo.Create(
            dto.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            dto.Description,
            (TodoPriority)dto.Priority,
            dto.DueDate)).ToList();

        await context.Todos.AddRangeAsync(todos, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new ImportTodosResult(todos.Count, importResult.FailureCount, importResult.Errors);
    }
}
