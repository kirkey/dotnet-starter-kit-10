using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Persistence;
using FSH.Framework.Shared.Multitenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FSH.Modules.Todo.Data;

/// <summary>
/// Initializes the Todo module database with schema and seed data.
/// 
/// **Purpose:**
/// Handles database migration and sample data seeding for the Todo module.
/// Supports both schema creation/migration and initial data population.
/// 
/// **Responsibilities:**
/// - Apply pending Entity Framework Core migrations
/// - Seed the database with sample Todo and TodoTask data
/// - Maintain tenant-specific database initialization
/// - Provide logging for database operations
/// 
/// **Migrations:**
/// - Runs all pending migrations when MigrateAsync is called
/// - Logs migration status per tenant
/// 
/// **Seeding:**
/// - Populates database with realistic sample todos and tasks if empty
/// - Creates todos with various priorities and statuses
/// - Marks some todos as in progress and some as completed
/// - Demonstrates multi-tenancy support
/// - Logs seeding status per tenant
/// </summary>
internal sealed class TodoDbInitializer(
    ILogger<TodoDbInitializer> logger,
    TodoDbContext context,
    IMultiTenantContextAccessor<AppTenantInfo> multiTenantContextAccessor) : IDbInitializer
{
    /// <summary>
    /// Applies pending Entity Framework Core migrations to the Todo database.
    /// 
    /// Executes all pending migrations for the current tenant's database.
    /// Logs the migration operation with tenant context for debugging.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken).ConfigureAwait(false)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] applied database migrations for todo module", 
                multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Identifier);
        }
    }

    /// <summary>
    /// Seeds the Todo database with sample data.
    /// 
    /// Populates the database with realistic sample todos and tasks if the database is empty.
    /// Includes:
    /// - 8 sample todos with varying priorities and statuses
    /// - Multiple tasks per todo demonstrating the master-detail relationship
    /// - Some todos marked as in progress with partial task completion
    /// - Some todos marked as fully completed with all tasks completed
    /// 
    /// Only runs once - if any todos already exist, seeding is skipped.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Helper method to create a Todo with associated tasks.
    /// 
    /// Simplifies the creation of sample todos by accepting an array of task names
    /// and automatically creating the Todo with all its tasks properly linked.
    /// </summary>
    /// <param name="name">The name of the todo.</param>
    /// <param name="description">The description of the todo.</param>
    /// <param name="priority">The priority level of the todo.</param>
    /// <param name="dueDate">The due date for the todo.</param>
    /// <param name="tenantId">The tenant ID for the todo.</param>
    /// <param name="createdBy">The user ID of the creator.</param>
    /// <param name="createdByUserName">The username of the creator.</param>
    /// <param name="taskNames">Array of task names to create for this todo.</param>
    /// <returns>A Todo instance with all specified tasks created and linked.</returns>
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
