using FluentValidation;
using FSH.Modules.Todo.Contracts.v1.TodoItems;

namespace FSH.Modules.Todo.Features.v1.TodoItems.UpdateTodoItem;

public class UpdateTodoItemCommandValidator : AbstractValidator<UpdateTodoItemCommand>
{
    public UpdateTodoItemCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Description).MaximumLength(1024);
        RuleFor(x => x.Notes).MaximumLength(2048);
        RuleFor(x => x.Priority).InclusiveBetween(0, 10);
    }
}
