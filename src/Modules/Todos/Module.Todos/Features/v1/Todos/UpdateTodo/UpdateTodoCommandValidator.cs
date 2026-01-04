using FluentValidation;
using FSH.Module.Todos.Contracts.v1.Todos;

namespace FSH.Module.Todos.Features.v1.Todos.UpdateTodo;

/// <summary>
/// Validator for UpdateTodoCommand.
/// 
/// **Purpose:**
/// Validates the UpdateTodoCommand request using FluentValidation.
/// All validation rules use constants from TodoStringLengths to ensure consistency.
/// 
/// **Validation Rules:**
/// - Id: Required (GUID)
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
public class UpdateTodoCommandValidator : AbstractValidator<UpdateTodoCommand>
{
    /// <summary>
    /// Initializes a new instance of UpdateTodoCommandValidator.
    /// 
    /// Sets up validation rules for updating an existing todo using centralized extensions.
    /// </summary>
    public UpdateTodoCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");

        RuleFor(x => x.Name).ValidateTodoName();
        RuleFor(x => x.Description).ValidateTodoDescription();
        RuleFor(x => x.Notes).ValidateTodoNotes();
        RuleFor(x => x.Priority).ValidatePriority();
    }
}
