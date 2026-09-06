using System.Net;
using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Framework.Jobs.Services;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Sources;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Domain;
using FSH.Modules.Ai.Jobs;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Features.v1.Sources.AddWebSource;

public sealed class AddWebSourceCommandHandler(
    AiDbContext db,
    IJobService jobs,
    ICurrentUser currentUser)
    : ICommandHandler<AddWebSourceCommand, Guid>
{
    public async ValueTask<Guid> Handle(AddWebSourceCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var tenantId = currentUser.GetTenant() ?? throw new UnauthorizedException("invalid tenant");
        var url = command.Url.Trim();
        var existing = await db.Sources
            .FirstOrDefaultAsync(s => s.SourceRef == url, cancellationToken)
            .ConfigureAwait(false);
        if (existing is not null)
        {
            throw new CustomException(
                $"This link is already ingested as source '{existing.Name}'.",
                errors: null,
                HttpStatusCode.Conflict);
        }

        var source = AiSource.CreateWebSource(
            string.IsNullOrWhiteSpace(command.Name) ? url : command.Name.Trim(), url);
        db.Sources.Add(source);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        jobs.Enqueue<WebSourceFetchJob>(j => j.FetchAsync(tenantId, source.Id, CancellationToken.None));

        return source.Id;
    }
}
