using FSH.Framework.Core.Exceptions;
using FSH.Modules.Todo.Contracts.v1.Todos;
using FSH.Modules.Todo.Data;
using FSH.Modules.Todo.Domain;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todo.Features.v1.Todos.UpdateTodoStatus;

public class UpdateTodoStatusCommandHandler(TodoDbContext dbContext) : ICommandHandler<UpdateTodoStatusCommand>
{
    public async ValueTask<Unit> Handle(UpdateTodoStatusCommand command, CancellationToken cancellationToken)
    {
        var todo = await dbContext.Todos
            .FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken)
            ?? throw new NotFoundException($"Todo with id {command.Id} not found.");

        if (!Enum.IsDefined(typeof(TodoStatus), command.Status))
        {
            throw new InvalidOperationException("Invalid status value.");
        }

        todo.UpdateStatus((TodoStatus)command.Status);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
