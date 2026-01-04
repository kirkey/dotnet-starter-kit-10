namespace Accounting.Application.TrialBalances.Search.v1;

/// <summary>
/// Handler for searching trial balance reports.
/// </summary>
public sealed class TrialBalanceSearchHandler(
    [FromKeyedServices("accounting:trial-balance")] IReadRepository<TrialBalance> repository,
    ILogger<TrialBalanceSearchHandler> logger)
    : IRequestHandler<TrialBalanceSearchRequest, PagedList<TrialBalanceSearchResponse>>
{
    public async Task<PagedList<TrialBalanceSearchResponse>> Handle(TrialBalanceSearchRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Searching trial balance reports with filters");

        var spec = new TrialBalanceSearchSpec(request);
        var trialBalances = await repository.ListAsync(spec, cancellationToken);
        var totalCount = await repository.CountAsync(spec, cancellationToken);

        logger.LogInformation("Found {Count} trial balance reports", trialBalances.Count);

        return new PagedList<TrialBalanceSearchResponse>(trialBalances, totalCount, request.PageNumber, request.PageSize);
    }
}
