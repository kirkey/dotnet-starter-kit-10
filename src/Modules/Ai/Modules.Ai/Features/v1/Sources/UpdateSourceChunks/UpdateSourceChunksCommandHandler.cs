using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Jobs.Services;
using FSH.Modules.Ai.Contracts.v1.Sources;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Jobs;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Sources.UpdateSourceChunks;

public sealed class UpdateSourceChunksCommandHandler(
    AiDbContext db,
    IJobService jobs,
    ICurrentUser currentUser)
    : ICommandHandler<UpdateSourceChunksCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateSourceChunksCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var tenantId = currentUser.GetTenant() ?? throw new UnauthorizedException("invalid tenant");
        var exists = await db.Sources
            .AnyAsync(s => s.Id == command.SourceId, cancellationToken)
            .ConfigureAwait(false);
        if (!exists)
        {
            throw new NotFoundException($"Source {command.SourceId} was not found.");
        }

        jobs.Enqueue<ChunkAndEmbedJob>(j => j.ProcessAsync(tenantId, command.SourceId, CancellationToken.None));

        return command.SourceId;
    }
}
