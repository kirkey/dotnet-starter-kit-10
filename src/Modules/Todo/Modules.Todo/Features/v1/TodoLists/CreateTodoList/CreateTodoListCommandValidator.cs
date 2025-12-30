using FluentValidation;
using FSH.Modules.Todo.Contracts.v1.TodoLists;

namespace FSH.Modules.Todo.Features.v1.TodoLists.CreateTodoList;

public class CreateTodoListCommandValidator : AbstractValidator<CreateTodoListCommand>
{
    public CreateTodoListCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.Notes)
            .MaximumLength(2000).When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage("Notes cannot exceed 2000 characters");

        RuleFor(x => x.Color)
            .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.Color))
            .WithMessage("Color cannot exceed 50 characters");
    }
}
