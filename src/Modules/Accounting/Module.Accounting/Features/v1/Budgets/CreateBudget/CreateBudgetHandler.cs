using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Budgets.CreateBudget;

/// <summary>
/// Command to create a new budget header.
/// </summary>
/// <param name="Name">Budget name or label (required)</param>
/// <param name="Description">Optional description for the budget</param>
public record CreateBudgetCommand(string Name, string? Description) : ICommand<Guid>;

/// <summary>
/// Handler for creating a Budget aggregate using the domain factory.
/// </summary>
/// <remarks>
/// Responsibility: Create budget header with tenant and creator audit fields, then persist to DB.
/// 
/// Execution Flow:
/// 1. Call Budget.Create(Name, tenant, userId, userName, Description)
/// 2. Add to DbSet and SaveChangesAsync
/// 3. Return created Budget Id
/// 
/// Permissions: Requires authenticated user with Budget.Create permission
/// 
/// Exceptions:
/// - DbException: Thrown if persistence fails
/// - ValidationException: Thrown by validators for invalid Name
/// </remarks>
public class CreateBudgetHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateBudgetCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateBudgetCommand command, CancellationToken ct)
    {
        var entity = Budget.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.Budgets.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
} 
