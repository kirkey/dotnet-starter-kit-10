using Asp.Versioning;
using Asp.Versioning.Builder;
using FSH.Framework.Persistence;
using FSH.Framework.Web.Modules;
using FSH.Modules.Todo.Data;
using FSH.Modules.Todo.Features.v1.TodoItems.AssignTodoItem;
using FSH.Modules.Todo.Features.v1.TodoItems.CompleteTodoItem;
using FSH.Modules.Todo.Features.v1.TodoItems.CreateTodoItem;
using FSH.Modules.Todo.Features.v1.TodoItems.DeleteTodoItem;
using FSH.Modules.Todo.Features.v1.TodoItems.GetTodoItem;
using FSH.Modules.Todo.Features.v1.TodoItems.GetTodoItems;
using FSH.Modules.Todo.Features.v1.TodoItems.UpdateTodoItem;
using FSH.Modules.Todo.Features.v1.TodoLists.CreateTodoList;
using FSH.Modules.Todo.Features.v1.TodoLists.DeleteTodoList;
using FSH.Modules.Todo.Features.v1.TodoLists.GetTodoList;
using FSH.Modules.Todo.Features.v1.TodoLists.GetTodoLists;
using FSH.Modules.Todo.Features.v1.TodoLists.UpdateTodoList;
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

        // TodoList endpoints
        group.MapCreateTodoListEndpoint();
        group.MapGetTodoListsEndpoint();
        group.MapGetTodoListEndpoint();
        group.MapUpdateTodoListEndpoint();
        group.MapDeleteTodoListEndpoint();

        // TodoItem endpoints
        group.MapCreateTodoItemEndpoint();
        group.MapGetTodoItemsEndpoint();
        group.MapGetTodoItemEndpoint();
        group.MapUpdateTodoItemEndpoint();
        group.MapDeleteTodoItemEndpoint();
        group.MapCompleteTodoItemEndpoint();
        group.MapAssignTodoItemEndpoint();
    }
}
