using FluentValidation;

namespace FSH.Modules.Todo.Features.v1.TodoTasks.UpdateTodoTask;

public class UpdateTodoTaskCommandValidator : AbstractValidator<FSH.Modules.Todo.Contracts.v1.TodoTasks.UpdateTodoTaskCommand>
{
    public UpdateTodoTaskCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("Sort order must be non-negative");
    }
}
