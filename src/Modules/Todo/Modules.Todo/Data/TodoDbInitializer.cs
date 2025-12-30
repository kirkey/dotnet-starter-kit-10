using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Core.Context;
using FSH.Framework.Persistence;
using FSH.Framework.Shared.Multitenancy;
using FSH.Modules.Todo.Domain;
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
        await SeedSampleTodoListsAsync(cancellationToken);
    }

    private async Task SeedSampleTodoListsAsync(CancellationToken cancellationToken)
    {
        string tenantId = multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Id ?? "root";
        
        // Check if sample data already exists
        if (await context.TodoLists.AnyAsync(cancellationToken))
        {
            logger.LogInformation("[{Tenant}] Todo sample data already exists, skipping seeding", tenantId);
            return;
        }

        var adminUserId = Guid.Parse("00000000-0000-0000-0000-000000000001"); // Default admin user ID
        var systemUserId = Guid.Parse("00000000-0000-0000-0000-000000000002"); // System user ID
        
        // Create sample lists using factory methods
        var list1 = TodoList.Create(
            "Build New Feature - User Dashboard",
            tenantId,
            adminUserId,
            "Admin",
            "Design and implement a comprehensive user dashboard with real-time analytics and notifications",
            "#4CAF50");
        list1.Notes = "High priority project for Q1 2025. Requires frontend, backend, and database work.";
        list1.SortOrder = 1;
        list1.DueDate = DateTimeOffset.UtcNow.AddDays(30);

        var list2 = TodoList.Create(
            "Critical Bug Fixes - Release v2.1",
            tenantId,
            systemUserId,
            "System",
            "Address reported bugs from production and fix performance issues",
            "#FF6B6B");
        list2.Notes = "Customer-reported issues with authentication flow and API response times";
        list2.SortOrder = 2;
        list2.DueDate = DateTimeOffset.UtcNow.AddDays(14);

        var list3 = TodoList.Create(
            "Documentation Updates",
            tenantId,
            adminUserId,
            "Admin",
            "Update API documentation, user guides, and developer onboarding materials",
            "#2196F3");
        list3.Notes = "Focus on comprehensive examples and troubleshooting guides";
        list3.SortOrder = 3;
        list3.DueDate = DateTimeOffset.UtcNow.AddDays(45);

        var list4 = TodoList.Create(
            "Infrastructure Improvements",
            tenantId,
            adminUserId,
            "Admin",
            "Upgrade servers, implement CI/CD pipeline, and optimize database indexes",
            "#FF9800");
        list4.Notes = "DevOps improvements for better deployment and monitoring";
        list4.SortOrder = 4;
        list4.DueDate = DateTimeOffset.UtcNow.AddDays(60);
        list4.CreatedOnUtc = DateTimeOffset.UtcNow.AddDays(-5);
        list4.LastModifiedOnUtc = DateTimeOffset.UtcNow.AddDays(-2);
        list4.LastModifiedBy = systemUserId;
        list4.LastModifiedByUserName = "System";

        var sampleTodoLists = new[] { list1, list2, list3, list4 };

        await context.TodoLists.AddRangeAsync(sampleTodoLists, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] Seeded {Count} sample todo lists", tenantId, sampleTodoLists.Length);

        // Add sample items to first todo list
        var item1 = TodoItem.Create(
            "Design UI mockups in Figma",
            list1.Id,
            adminUserId,
            "Admin",
            "Create high-fidelity mockups for all dashboard pages including desktop and mobile views",
            TodoPriority.High);
        item1.Notes = "Review with design team for feedback before implementation";
        item1.SortOrder = 1;
        item1.DueDate = DateTimeOffset.UtcNow.AddDays(-5);
        item1.CompletedDate = DateTimeOffset.UtcNow.AddDays(-3);
        item1.Status = TodoItemStatus.Completed;
        item1.CreatedOnUtc = DateTimeOffset.UtcNow.AddDays(-10);
        item1.LastModifiedOnUtc = DateTimeOffset.UtcNow.AddDays(-3);
        item1.LastModifiedBy = adminUserId;
        item1.LastModifiedByUserName = "Admin";
        item1.AssignedToUserId = adminUserId.ToString();
        item1.AssignedToUserName = "John Doe";

        var item2 = TodoItem.Create(
            "Setup database schema and migrations",
            list1.Id,
            systemUserId,
            "System",
            "Create EF Core models, DbContext, and migrations for dashboard data storage",
            TodoPriority.Critical);
        item2.Notes = "Ensure proper indexing for performance with large datasets";
        item2.SortOrder = 2;
        item2.DueDate = DateTimeOffset.UtcNow.AddDays(5);
        item2.Status = TodoItemStatus.InProgress;
        item2.EstimatedHours = 8;
        item2.ActualHours = 6;
        item2.CreatedOnUtc = DateTimeOffset.UtcNow.AddDays(-8);
        item2.LastModifiedOnUtc = DateTimeOffset.UtcNow.AddHours(-2);
        item2.LastModifiedBy = systemUserId;
        item2.LastModifiedByUserName = "System";
        item2.AssignedToUserId = systemUserId.ToString();
        item2.AssignedToUserName = "Jane Smith";

        var item3 = TodoItem.Create(
            "Implement API endpoints for dashboard data",
            list1.Id,
            adminUserId,
            "Admin",
            "Create REST API endpoints to fetch user analytics, metrics, and real-time notifications",
            TodoPriority.Critical);
        item3.Notes = "Must implement proper pagination and caching for performance";
        item3.SortOrder = 3;
        item3.DueDate = DateTimeOffset.UtcNow.AddDays(10);
        item3.EstimatedHours = 12;
        item3.CreatedOnUtc = DateTimeOffset.UtcNow.AddDays(-5);

        var item4 = TodoItem.Create(
            "Build React/Blazor frontend components",
            list1.Id,
            systemUserId,
            "System",
            "Develop reusable dashboard components with charts, tables, and widgets",
            TodoPriority.High);
        item4.Notes = "Use existing component library for consistency";
        item4.SortOrder = 4;
        item4.DueDate = DateTimeOffset.UtcNow.AddDays(15);
        item4.EstimatedHours = 16;
        item4.CreatedOnUtc = DateTimeOffset.UtcNow.AddDays(-4);

        var item5 = TodoItem.Create(
            "Implement real-time notifications",
            list1.Id,
            adminUserId,
            "Admin",
            "Add WebSocket support for real-time push notifications to users",
            TodoPriority.Medium);
        item5.Notes = "Integrate with notification service and ensure scalability";
        item5.SortOrder = 5;
        item5.DueDate = DateTimeOffset.UtcNow.AddDays(20);
        item5.EstimatedHours = 10;
        item5.CreatedOnUtc = DateTimeOffset.UtcNow.AddDays(-2);

        var item6 = TodoItem.Create(
            "Write unit and integration tests",
            list1.Id,
            systemUserId,
            "System",
            "Achieve 80%+ code coverage with comprehensive test cases",
            TodoPriority.High);
        item6.Notes = "Include edge cases and error scenarios";
        item6.SortOrder = 6;
        item6.DueDate = DateTimeOffset.UtcNow.AddDays(25);
        item6.EstimatedHours = 14;
        item6.CreatedOnUtc = DateTimeOffset.UtcNow.AddDays(-1);

        var sampleTodoItems = new[] { item1, item2, item3, item4, item5, item6 };

        await context.TodoItems.AddRangeAsync(sampleTodoItems, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("[{Tenant}] Seeded {Count} sample todo items for list '{ListName}'", 
            tenantId, sampleTodoItems.Length, list1.Name);
    }
}
