using Asp.Versioning;
using FSH.Framework.Persistence;
using FSH.Framework.Web.Modules;
using FSH.Modules.Todo.Data;
using FSH.Modules.Todo.Features.v1.TodoItems.CreateTodoItem;
using FSH.Modules.Todo.Features.v1.TodoLists.CreateTodoList;
using FSH.Modules.Todo.Features.v1.TodoLists.GetTodoList;
using FSH.Modules.Todo.Features.v1.TodoLists.GetTodoLists;
using FSH.Modules.Todo.Features.v1.TodoLists.UpdateTodoList;
using Microsoft.AspNetCore.Builder;
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
        // API versioning
        var versionSet = endpoints.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        // Route group
        var group = endpoints.MapGroup("api/v{version:apiVersion}/todo")
            .WithApiVersionSet(versionSet);

        // Map TodoList endpoints
        group.MapCreateTodoListEndpoint();
        group.MapGetTodoListsEndpoint();
        group.MapGetTodoListEndpoint();
        group.MapUpdateTodoListEndpoint();

        // Map TodoItem endpoints
        group.MapCreateTodoItemEndpoint();
    }
}
