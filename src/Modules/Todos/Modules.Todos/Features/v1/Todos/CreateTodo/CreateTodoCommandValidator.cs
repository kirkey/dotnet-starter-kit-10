using FluentValidation;
using FSH.Modules.Todos.Contracts.v1.Todos;

namespace FSH.Modules.Todos.Features.v1.Todos.CreateTodo;

/// <summary>
/// Validator for CreateTodoCommand.
/// 
/// **Purpose:**
/// Validates the CreateTodoCommand request using FluentValidation.
/// All validation rules use constants from TodoStringLengths to ensure consistency.
/// 
/// **Validation Rules:**
/// - Name: Required, max 128 chars
/// - Description: Optional, max 512 chars
/// - Notes: Optional, max 2048 chars
/// - Priority: Required, between 1-4
/// 
/// **Benefits:**
/// - Uses TodoValidationExtensions for DRY code
/// - All rules reference TodoStringLengths constants
/// - Consistent with database configuration
/// - Easy to maintain and update
/// </summary>
public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
{
    /// <summary>
    /// Initializes a new instance of CreateTodoCommandValidator.
    /// 
    /// Sets up validation rules for creating a new todo using centralized extensions.
    /// </summary>
    public CreateTodoCommandValidator()
    {
        RuleFor(x => x.Name).ValidateTodoName();
        RuleFor(x => x.Description).ValidateTodoDescription();
        RuleFor(x => x.Notes).ValidateTodoNotes();
        RuleFor(x => x.Priority).ValidatePriority();
    }
}
