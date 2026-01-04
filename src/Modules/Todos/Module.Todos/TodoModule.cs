using Asp.Versioning;
using Asp.Versioning.Builder;
using FSH.Framework.Persistence;
using FSH.Framework.Shared.Identity;
using FSH.Framework.Web.Modules;
using FSH.Module.Todos.Data;
using FSH.Module.Todos.Features.v1.Todos.ArchiveTodo;
using FSH.Module.Todos.Features.v1.Todos.CompleteTodo;
using FSH.Module.Todos.Features.v1.Todos.CreateTodo;
using FSH.Module.Todos.Features.v1.Todos.DeleteTodo;
using FSH.Module.Todos.Features.v1.Todos.ExportTodos;
using FSH.Module.Todos.Features.v1.Todos.GetTodo;
using FSH.Module.Todos.Features.v1.Todos.GetTodos;
using FSH.Module.Todos.Features.v1.Todos.ImportTodos;
using FSH.Module.Todos.Features.v1.Todos.ReopenTodo;
using FSH.Module.Todos.Features.v1.Todos.UpdateTodo;
using FSH.Module.Todos.Features.v1.Todos.UpdateTodoStatus;
using FSH.Module.Todos.Features.v1.TodoTasks.CreateTodoTask;
using FSH.Module.Todos.Features.v1.TodoTasks.DeleteTodoTask;
using FSH.Module.Todos.Features.v1.TodoTasks.GetTodoTasks;
using FSH.Module.Todos.Features.v1.TodoTasks.ReorderTasks;
using FSH.Module.Todos.Features.v1.TodoTasks.ToggleTaskCompletion;
using FSH.Module.Todos.Features.v1.TodoTasks.UpdateTodoTask;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace FSH.Module.Todos;

/// <summary>
/// Todo Module - A comprehensive module for managing todo items and tasks.
/// 
/// **Purpose:**
/// Provides a complete feature set for creating, managing, and tracking todo items with support for:
/// - Master-Detail relationship between Todos and TodoTasks
/// - Status tracking and priority management
/// - Completion tracking with timestamps
/// - Archiving functionality for inactive todos
/// - Bulk import/export operations
/// - Full audit trail for compliance and tracking
/// 
/// **Key Features:**
/// - Create, read, update, and delete todo items
/// - Add and manage tasks within each todo
/// - Track completion status and completion dates
/// - Set priority levels (None, Low, Medium, High, Critical)
/// - Manage todo status (NotStarted, InProgress, Completed, OnHold)
/// - Reorder tasks with sort order support
/// - Archive completed or inactive todos
/// - Export todos to file formats
/// - Import todos from external sources
/// 
/// **Architecture:**
/// - Uses Entity Framework Core with PostgreSQL/MSSQL support
/// - Implements Domain-Driven Design with aggregate pattern
/// - Supports multi-tenancy through Finbuckle
/// - Uses CQRS pattern with Mediator library for commands/queries
/// - Includes FluentValidation for request validation
/// 
/// **Entities:**
/// - Todo: Master entity representing a todo item with status, priority, and completion tracking
/// - TodoTask: Detail entity representing a task within a todo
/// 
/// **Permissions:**
/// - Todos.View: View todo items
/// - Todos.Search: Search todo items
/// - Todos.Create: Create new todo items
/// - Todos.Update: Update existing todo items
/// - Todos.Delete: Delete todo items
/// - Todos.Export: Export todos
/// - Todos.Import: Import todos
/// </summary>
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
