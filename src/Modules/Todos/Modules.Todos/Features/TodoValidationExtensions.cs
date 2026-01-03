using FluentValidation;

namespace FSH.Modules.Todos.Features;

/// <summary>
/// Centralized validation rules for Todo module using constants.
/// 
/// **Purpose:**
/// Provides reusable FluentValidation extension methods for common Todo properties.
/// All rules use constants from TodoStringLengths to ensure consistency across validators.
/// 
/// **Benefits:**
/// - Single source of truth for validation rules
/// - DRY principle compliance
/// - Easy to maintain and update
/// - Fluent API for clean validator definitions
/// - Automatic consistency with database constraints
/// 
/// **Usage Pattern:**
/// RuleFor(x => x.Name).ValidateTodoName();
/// RuleFor(x => x.Description).ValidateTodoDescription();
/// RuleFor(x => x.Notes).ValidateTodoNotes();
/// 
/// **Naming Convention:**
/// Validate{Entity}{Property}() - e.g., ValidateTodoName, ValidateTodoTaskDescription
/// </summary>
public static class TodoValidationExtensions
{
    // ========== Todo Property Validations ==========

    /// <summary>
    /// Applies validation rules for Todo name property.
    /// 
    /// Rules:
    /// - Must not be empty
    /// - Must not exceed TodoStringLengths.TodoNameMaxLength (128 chars)
    /// 
    /// Usage:
    /// RuleFor(x => x.Name).ValidateTodoName();
    /// </summary>
    /// <typeparam name="T">The command/DTO type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the property.</param>
    /// <returns>The configured rule builder for method chaining.</returns>
    public static IRuleBuilderOptions<T, string> ValidateTodoName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("Todo name is required")
            .MaximumLength(TodoStringLengths.TodoNameMaxLength)
            .WithMessage($"Todo name must not exceed {TodoStringLengths.TodoNameMaxLength} characters");
    }

    /// <summary>
    /// Applies validation rules for Todo description property.
    /// 
    /// Rules:
    /// - Optional field
    /// - Must not exceed TodoStringLengths.TodoDescriptionMaxLength (512 chars)
    /// 
    /// Usage:
    /// RuleFor(x => x.Description).ValidateTodoDescription();
    /// </summary>
    /// <typeparam name="T">The command/DTO type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the property.</param>
    /// <returns>The configured rule builder for method chaining.</returns>
    public static IRuleBuilderOptions<T, string?> ValidateTodoDescription<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(TodoStringLengths.TodoDescriptionMaxLength)
            .WithMessage($"Todo description must not exceed {TodoStringLengths.TodoDescriptionMaxLength} characters")
            .When(x => x != null);
    }

    /// <summary>
    /// Applies validation rules for Todo notes property.
    /// 
    /// Rules:
    /// - Optional field
    /// - Must not exceed TodoStringLengths.TodoNotesMaxLength (2048 chars)
    /// 
    /// Usage:
    /// RuleFor(x => x.Notes).ValidateTodoNotes();
    /// </summary>
    /// <typeparam name="T">The command/DTO type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the property.</param>
    /// <returns>The configured rule builder for method chaining.</returns>
    public static IRuleBuilderOptions<T, string?> ValidateTodoNotes<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(TodoStringLengths.TodoNotesMaxLength)
            .WithMessage($"Todo notes must not exceed {TodoStringLengths.TodoNotesMaxLength} characters")
            .When(x => x != null);
    }

    // ========== TodoTask Property Validations ==========

    /// <summary>
    /// Applies validation rules for TodoTask name property.
    /// 
    /// Rules:
    /// - Must not be empty
    /// - Must not exceed TodoStringLengths.TodoTaskNameMaxLength (128 chars)
    /// 
    /// Usage:
    /// RuleFor(x => x.Name).ValidateTodoTaskName();
    /// </summary>
    /// <typeparam name="T">The command/DTO type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the property.</param>
    /// <returns>The configured rule builder for method chaining.</returns>
    public static IRuleBuilderOptions<T, string> ValidateTodoTaskName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("Task name is required")
            .MaximumLength(TodoStringLengths.TodoTaskNameMaxLength)
            .WithMessage($"Task name must not exceed {TodoStringLengths.TodoTaskNameMaxLength} characters");
    }

    /// <summary>
    /// Applies validation rules for TodoTask description property.
    /// 
    /// Rules:
    /// - Optional field
    /// - Must not exceed TodoStringLengths.TodoTaskDescriptionMaxLength (512 chars)
    /// 
    /// Usage:
    /// RuleFor(x => x.Description).ValidateTodoTaskDescription();
    /// </summary>
    /// <typeparam name="T">The command/DTO type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the property.</param>
    /// <returns>The configured rule builder for method chaining.</returns>
    public static IRuleBuilderOptions<T, string?> ValidateTodoTaskDescription<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(TodoStringLengths.TodoTaskDescriptionMaxLength)
            .WithMessage($"Task description must not exceed {TodoStringLengths.TodoTaskDescriptionMaxLength} characters")
            .When(x => x != null);
    }

    /// <summary>
    /// Applies validation rules for TodoTask notes property.
    /// 
    /// Rules:
    /// - Optional field
    /// - Must not exceed TodoStringLengths.TodoTaskNotesMaxLength (2048 chars)
    /// 
    /// Usage:
    /// RuleFor(x => x.Notes).ValidateTodoTaskNotes();
    /// </summary>
    /// <typeparam name="T">The command/DTO type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the property.</param>
    /// <returns>The configured rule builder for method chaining.</returns>
    public static IRuleBuilderOptions<T, string?> ValidateTodoTaskNotes<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(TodoStringLengths.TodoTaskNotesMaxLength)
            .WithMessage($"Task notes must not exceed {TodoStringLengths.TodoTaskNotesMaxLength} characters")
            .When(x => x != null);
    }

    /// <summary>
    /// Applies validation rules for TodoTask sort order property.
    /// 
    /// Rules:
    /// - Must be a non-negative integer
    /// - Used for task ordering within a todo
    /// 
    /// Usage:
    /// RuleFor(x => x.SortOrder).ValidateSortOrder();
    /// </summary>
    /// <typeparam name="T">The command/DTO type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the property.</param>
    /// <returns>The configured rule builder for method chaining.</returns>
    public static IRuleBuilderOptions<T, int> ValidateSortOrder<T>(
        this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThanOrEqualTo(0)
            .WithMessage("Sort order must be a non-negative number");
    }

    // ========== Shared/Common Validations ==========

    /// <summary>
    /// Applies validation rules for priority field (int 1-4).
    /// 
    /// Rules:
    /// - Must be between 1 and 4
    /// - 1 = Low, 2 = Medium, 3 = High, 4 = Critical
    /// 
    /// Usage:
    /// RuleFor(x => x.Priority).ValidatePriority();
    /// </summary>
    /// <typeparam name="T">The command/DTO type being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the property.</param>
    /// <returns>The configured rule builder for method chaining.</returns>
    public static IRuleBuilderOptions<T, int> ValidatePriority<T>(
        this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .InclusiveBetween(1, 4)
            .WithMessage("Priority must be between 1 (Low) and 4 (Critical)");
    }
}
