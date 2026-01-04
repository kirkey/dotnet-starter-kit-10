using FSH.Framework.Core.Exceptions;
using FSH.Module.Todos.Contracts.v1.Todos;
using FSH.Module.Todos.Data;
using FSH.Module.Todos.Domain;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Todos.Features.v1.Todos.UpdateTodoStatus;

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
