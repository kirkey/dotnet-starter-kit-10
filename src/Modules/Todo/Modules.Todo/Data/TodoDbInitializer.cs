using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Core.Context;
using FSH.Framework.Persistence;
using FSH.Framework.Shared.Multitenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FSH.Modules.Todo.Data;

internal sealed class TodoDbInitializer(
    ILogger<TodoDbInitializer> logger,
    TodoDbContext context,
    IMultiTenantContextAccessor<AppTenantInfo> multiTenantContextAccessor) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken).ConfigureAwait(false)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] applied database migrations for todo module", 
                multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Identifier);
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        if (await context.Todos.AnyAsync(cancellationToken))
        {
            logger.LogInformation("[{Tenant}] todo data already seeded", 
                multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Identifier);
            return;
        }

        var tenantId = multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Identifier ?? "root";
        var systemUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        const string systemUserName = "System";

        // Create sample todos with tasks
        var todos = new List<Domain.Todo>
        {
            CreateTodoWithTasks(
                "Setup Development Environment",
                "Configure local development environment for the project",
                Domain.TodoPriority.High,
                DateTimeOffset.UtcNow.AddDays(2),
                tenantId,
                systemUserId,
                systemUserName,
                new[]
                {
                    "Install .NET 10 SDK",
                    "Clone repository from GitHub",
                    "Install Docker Desktop",
                    "Setup PostgreSQL database",
                    "Run database migrations",
                    "Install IDE extensions"
                }),

            CreateTodoWithTasks(
                "Complete User Authentication Module",
                "Implement JWT authentication and authorization features",
                Domain.TodoPriority.Critical,
                DateTimeOffset.UtcNow.AddDays(5),
                tenantId,
                systemUserId,
                systemUserName,
                new[]
                {
                    "Design authentication flow",
                    "Implement JWT token generation",
                    "Add refresh token mechanism",
                    "Create login/logout endpoints",
                    "Add password reset functionality",
                    "Write unit tests"
                }),

            CreateTodoWithTasks(
                "Prepare Quarterly Report",
                "Compile and analyze Q4 2025 performance metrics",
                Domain.TodoPriority.Medium,
                DateTimeOffset.UtcNow.AddDays(10),
                tenantId,
                systemUserId,
                systemUserName,
                new[]
                {
                    "Gather sales data",
                    "Analyze customer feedback",
                    "Create visualizations",
                    "Write executive summary",
                    "Review with team"
                }),

            CreateTodoWithTasks(
                "Code Review Best Practices",
                "Document and share code review guidelines with the team",
                Domain.TodoPriority.Low,
                DateTimeOffset.UtcNow.AddDays(15),
                tenantId,
                systemUserId,
                systemUserName,
                new[]
                {
                    "Research industry standards",
                    "Draft initial guidelines",
                    "Get team feedback",
                    "Create examples",
                    "Publish to wiki"
                }),

            CreateTodoWithTasks(
                "API Documentation",
                "Complete OpenAPI/Swagger documentation for all endpoints",
                Domain.TodoPriority.Medium,
                DateTimeOffset.UtcNow.AddDays(7),
                tenantId,
                systemUserId,
                systemUserName,
                new[]
                {
                    "Document authentication endpoints",
                    "Document user management endpoints",
                    "Document tenant endpoints",
                    "Add request/response examples",
                    "Review and publish"
                }),

            CreateTodoWithTasks(
                "Performance Optimization",
                "Optimize database queries and API response times",
                Domain.TodoPriority.High,
                DateTimeOffset.UtcNow.AddDays(12),
                tenantId,
                systemUserId,
                systemUserName,
                new[]
                {
                    "Profile slow queries",
                    "Add database indexes",
                    "Implement caching strategy",
                    "Optimize N+1 queries",
                    "Load test endpoints",
                    "Document improvements"
                }),

            CreateTodoWithTasks(
                "Team Onboarding Process",
                "Create comprehensive onboarding guide for new developers",
                Domain.TodoPriority.Low,
                DateTimeOffset.UtcNow.AddDays(20),
                tenantId,
                systemUserId,
                systemUserName,
                new[]
                {
                    "Document project architecture",
                    "Create setup guide",
                    "Record walkthrough videos",
                    "Prepare sample tasks",
                    "Get feedback from recent hires"
                }),

            CreateTodoWithTasks(
                "Security Audit",
                "Conduct comprehensive security review of the application",
                Domain.TodoPriority.Critical,
                DateTimeOffset.UtcNow.AddDays(3),
                tenantId,
                systemUserId,
                systemUserName,
                new[]
                {
                    "Review authentication mechanisms",
                    "Check authorization rules",
                    "Scan for SQL injection",
                    "Test XSS vulnerabilities",
                    "Review dependency vulnerabilities",
                    "Update security documentation"
                })
        };

        // Mark some todos as in progress or completed
        todos[0].UpdateStatus(Domain.TodoStatus.InProgress);
        todos[0].Tasks.ElementAt(0).Complete();
        todos[0].Tasks.ElementAt(1).Complete();
        todos[0].Tasks.ElementAt(2).Complete();

        todos[2].UpdateStatus(Domain.TodoStatus.InProgress);
        todos[2].Tasks.ElementAt(0).Complete();
        todos[2].Tasks.ElementAt(1).Complete();

        todos[6].Complete();
        foreach (var task in todos[6].Tasks)
        {
            task.Complete();
        }

        context.Todos.AddRange(todos);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] seeded {Count} todo items with tasks", 
            multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Identifier, 
            todos.Count);
    }

    private static Domain.Todo CreateTodoWithTasks(
        string name,
        string description,
        Domain.TodoPriority priority,
        DateTimeOffset dueDate,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string[] taskNames)
    {
        var todo = Domain.Todo.Create(
            name,
            tenantId,
            createdBy,
            createdByUserName,
            description,
            priority,
            dueDate);

        for (int i = 0; i < taskNames.Length; i++)
        {
            var task = Domain.TodoTask.Create(
                todo.Id,
                taskNames[i],
                tenantId,
                createdBy,
                createdByUserName,
                null,
                i);

            todo.Tasks.Add(task);
        }

        return todo;
    }
}
