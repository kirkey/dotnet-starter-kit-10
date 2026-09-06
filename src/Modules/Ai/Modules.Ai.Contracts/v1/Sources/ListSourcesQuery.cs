using FSH.Framework.Shared.Persistence;
using FSH.Modules.Ai.Contracts.Dtos;
using Mediator;

namespace FSH.Modules.Ai.Contracts.v1.Sources;

public sealed record ListSourcesQuery : IQuery<PagedResponse<AiSourceDto>>, IPagedQuery
{
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
    public string? Sort { get; set; }
    public AiSourceKind? Kind { get; set; }
    public AiSourceStatus? Status { get; set; }
    public string? Search { get; set; }
}
