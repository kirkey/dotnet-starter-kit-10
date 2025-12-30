using FluentValidation;
using FSH.Modules.Todo.Contracts.v1.TodoItems;

namespace FSH.Modules.Todo.Features.v1.TodoItems.CreateTodoItem;

public class CreateTodoItemCommandValidator : AbstractValidator<CreateTodoItemCommand>
{
    public CreateTodoItemCommandValidator()
    {
        RuleFor(x => x.TodoListId)
            .NotEmpty().WithMessage("TodoListId is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.Notes)
            .MaximumLength(2000).When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage("Notes cannot exceed 2000 characters");

        RuleFor(x => x.Priority)
            .InclusiveBetween(1, 4).WithMessage("Priority must be between 1 (Low) and 4 (Critical)");

        RuleFor(x => x.EstimatedHours)
            .GreaterThan(0).When(x => x.EstimatedHours.HasValue)
            .WithMessage("Estimated hours must be positive");
    }
}
