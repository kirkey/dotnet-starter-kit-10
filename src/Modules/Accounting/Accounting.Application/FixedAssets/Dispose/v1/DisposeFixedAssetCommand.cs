namespace Accounting.Application.FixedAssets.Dispose.v1;

public sealed record DisposeFixedAssetCommand(
    DefaultIdType Id,
    DateTime DisposalDate,
    decimal? DisposalAmount,
    string? DisposalReason
) : IRequest<DefaultIdType>;
