using FluentValidation;
using FSH.Modules.Todos.Contracts.v1.TodoTasks;

namespace FSH.Modules.Todos.Features.v1.TodoTasks.CreateTodoTask;

/// <summary>
/// Validator for CreateTodoTaskCommand.
/// 
/// **Purpose:**
/// Validates the CreateTodoTaskCommand request using FluentValidation.
/// All validation rules use constants from TodoStringLengths to ensure consistency.
/// 
/// **Validation Rules:**
/// - TodoId: Required (parent GUID)
/// - Name: Required, max 128 chars
/// - Description: Optional, max 512 chars
/// - SortOrder: Optional, must be non-negative
/// 
/// **Benefits:**
/// - Uses TodoValidationExtensions for DRY code
/// - All rules reference TodoStringLengths constants
/// - Consistent with database configuration
/// - Easy to maintain and update
/// </summary>
public class CreateTodoTaskCommandValidator : AbstractValidator<CreateTodoTaskCommand>
{
    /// <summary>
    /// Initializes a new instance of CreateTodoTaskCommandValidator.
    /// 
    /// Sets up validation rules for creating a new task using centralized extensions.
    /// </summary>
    public CreateTodoTaskCommandValidator()
    {
        RuleFor(x => x.TodoId)
            .NotEmpty().WithMessage("TodoId is required");

        RuleFor(x => x.Name).ValidateTodoTaskName();
        RuleFor(x => x.Description).ValidateTodoTaskDescription();
        RuleFor(x => x.SortOrder).ValidateSortOrder();
    }
}
