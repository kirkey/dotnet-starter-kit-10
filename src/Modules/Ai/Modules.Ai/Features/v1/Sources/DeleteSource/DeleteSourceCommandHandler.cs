using FSH.Framework.Core.Exceptions;
using FSH.Framework.Storage.Services;
using FSH.Modules.Ai.Contracts.v1.Sources;
using FSH.Modules.Ai.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FSH.Modules.Ai.Features.v1.Sources.DeleteSource;

/// <summary>
/// Hard-deletes a source: chunks via bulk delete (bypasses the soft-delete interceptor),
/// the Markdown twin from storage (best-effort), then the source row itself. File originals
/// stay owned by the Files module.
/// </summary>
public sealed class DeleteSourceCommandHandler(
    AiDbContext db,
    IStorageService storage,
    ILogger<DeleteSourceCommandHandler> logger)
    : ICommandHandler<DeleteSourceCommand, Guid>
{
    public async ValueTask<Guid> Handle(DeleteSourceCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var source = await db.Sources
            .FirstOrDefaultAsync(s => s.Id == command.SourceId, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Source {command.SourceId} was not found.");

        await db.Chunks
            .Where(c => c.SourceId == source.Id)
            .ExecuteDeleteAsync(cancellationToken)
            .ConfigureAwait(false);

        if (source.MdStorageKey is not null)
        {
            try
            {
                await storage.RemoveAsync(source.MdStorageKey, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "[Ai] could not remove twin {Key} for deleted source {SourceId}", source.MdStorageKey, source.Id);
            }
        }

        await db.Sources
            .Where(s => s.Id == source.Id)
            .ExecuteDeleteAsync(cancellationToken)
            .ConfigureAwait(false);

        return source.Id;
    }
}
