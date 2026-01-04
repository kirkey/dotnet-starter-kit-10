using FluentValidation;
using FSH.Module.Todos.Contracts.v1.TodoTasks;

namespace FSH.Module.Todos.Features.v1.TodoTasks.UpdateTodoTask;

/// <summary>
/// Validator for UpdateTodoTaskCommand.
/// 
/// **Purpose:**
/// Validates the UpdateTodoTaskCommand request using FluentValidation.
/// All validation rules use constants from TodoStringLengths to ensure consistency.
/// 
/// **Validation Rules:**
/// - Id: Required (GUID)
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
public class UpdateTodoTaskCommandValidator : AbstractValidator<UpdateTodoTaskCommand>
{
    /// <summary>
    /// Initializes a new instance of UpdateTodoTaskCommandValidator.
    /// 
    /// Sets up validation rules for updating an existing task using centralized extensions.
    /// </summary>
    public UpdateTodoTaskCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");

        RuleFor(x => x.Name).ValidateTodoTaskName();
        RuleFor(x => x.Description).ValidateTodoTaskDescription();
        RuleFor(x => x.SortOrder).ValidateSortOrder();
    }
}
