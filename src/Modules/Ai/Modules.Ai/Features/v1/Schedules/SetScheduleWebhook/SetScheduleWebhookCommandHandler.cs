using FSH.Framework.Core.Exceptions;
using FSH.Modules.Ai.Contracts.v1.Schedules;
using FSH.Modules.Ai.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Schedules.SetScheduleWebhook;

public sealed class SetScheduleWebhookCommandHandler(AiDbContext db)
    : ICommandHandler<SetScheduleWebhookCommand, string>
{
    public async ValueTask<string> Handle(SetScheduleWebhookCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var schedule = await db.AgentSchedules
            .FirstOrDefaultAsync(s => s.Id == command.Id, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Schedule {command.Id} was not found.");

        schedule.RotateWebhookToken();
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return schedule.WebhookToken;
    }
}
