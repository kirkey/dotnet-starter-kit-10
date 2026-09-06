using System.Net;
using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Jobs.Services;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Sources;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Jobs;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Sources.RefreshWebSource;

public sealed class RefreshWebSourceCommandHandler(
    AiDbContext db,
    IJobService jobs,
    ICurrentUser currentUser)
    : ICommandHandler<RefreshWebSourceCommand, Guid>
{
    public async ValueTask<Guid> Handle(RefreshWebSourceCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var tenantId = currentUser.GetTenant() ?? throw new UnauthorizedException("invalid tenant");
        var source = await db.Sources
            .FirstOrDefaultAsync(s => s.Id == command.SourceId, cancellationToken)
            .ConfigureAwait(false)
            ?? throw new NotFoundException($"Source {command.SourceId} was not found.");
        if (source.Kind != AiSourceKind.WebLink)
        {
            throw new CustomException(
                "Only web-link sources can be refreshed.",
                errors: null,
                HttpStatusCode.BadRequest);
        }

        source.MarkPending();
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        jobs.Enqueue<WebSourceFetchJob>(j => j.FetchAsync(tenantId, source.Id, CancellationToken.None));

        return source.Id;
    }
}
