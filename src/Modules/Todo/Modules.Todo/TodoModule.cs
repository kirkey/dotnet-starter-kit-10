using Asp.Versioning;
using Asp.Versioning.Builder;
using FSH.Framework.Persistence;
using FSH.Framework.Shared.Identity;
using FSH.Framework.Web.Modules;
using FSH.Modules.Todo.Data;
using FSH.Modules.Todo.Features.v1.Todos.ArchiveTodo;
using FSH.Modules.Todo.Features.v1.Todos.CompleteTodo;
using FSH.Modules.Todo.Features.v1.Todos.CreateTodo;
using FSH.Modules.Todo.Features.v1.Todos.DeleteTodo;
using FSH.Modules.Todo.Features.v1.Todos.ExportTodos;
using FSH.Modules.Todo.Features.v1.Todos.GetTodo;
using FSH.Modules.Todo.Features.v1.Todos.GetTodos;
using FSH.Modules.Todo.Features.v1.Todos.ImportTodos;
using FSH.Modules.Todo.Features.v1.Todos.ReopenTodo;
using FSH.Modules.Todo.Features.v1.Todos.UpdateTodo;
using FSH.Modules.Todo.Features.v1.Todos.UpdateTodoStatus;
using FSH.Modules.Todo.Features.v1.TodoTasks.CreateTodoTask;
using FSH.Modules.Todo.Features.v1.TodoTasks.DeleteTodoTask;
using FSH.Modules.Todo.Features.v1.TodoTasks.GetTodoTasks;
using FSH.Modules.Todo.Features.v1.TodoTasks.ReorderTasks;
using FSH.Modules.Todo.Features.v1.TodoTasks.ToggleTaskCompletion;
using FSH.Modules.Todo.Features.v1.TodoTasks.UpdateTodoTask;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace FSH.Modules.Todo;

public class TodoModule : IModule
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        // Register permissions
        PermissionConstants.Register(TodoPermissionConstants.GetPermissions());

        // Register DbContext
        builder.Services.AddHeroDbContext<TodoDbContext>();

        // Register Db Initializer
        builder.Services.AddScoped<IDbInitializer, TodoDbInitializer>();

        // Health checks
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<TodoDbContext>(
                name: "db:todo",
                failureStatus: HealthStatus.Unhealthy);
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        ApiVersionSet apiVersionSet = endpoints.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        RouteGroupBuilder group = endpoints
            .MapGroup("api/v{version:apiVersion}/todo")
            .WithTags("Todos")
            .WithApiVersionSet(apiVersionSet);

        // Todo endpoints
        group.MapGetTodosEndpoint();
        group.MapGetTodoEndpoint();
        group.MapCreateTodoEndpoint();
        group.MapUpdateTodoEndpoint();
        group.MapDeleteTodoEndpoint();
        group.MapCompleteTodoEndpoint();
        group.MapReopenTodoEndpoint();
        group.MapUpdateTodoStatusEndpoint();
        group.MapArchiveTodoEndpoint();
        group.MapExportTodosEndpoint();
        group.MapImportTodosEndpoint();

        // TodoTask endpoints
        group.MapGetTodoTasksEndpoint();
        group.MapCreateTodoTaskEndpoint();
        group.MapUpdateTodoTaskEndpoint();
        group.MapDeleteTodoTaskEndpoint();
        group.MapToggleTaskCompletionEndpoint();
        group.MapReorderTasksEndpoint();
    }
}
