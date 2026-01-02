using FluentValidation;

namespace FSH.Modules.Todo.Features.v1.TodoTasks.CreateTodoTask;

public class CreateTodoTaskCommandValidator : AbstractValidator<FSH.Modules.Todo.Contracts.v1.TodoTasks.CreateTodoTaskCommand>
{
    public CreateTodoTaskCommandValidator()
    {
        RuleFor(x => x.TodoId)
            .NotEmpty().WithMessage("TodoId is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

        RuleFor(x => x.SortOrder)
            .GreaterThanOrEqualTo(0).WithMessage("Sort order must be non-negative");
    }
}
