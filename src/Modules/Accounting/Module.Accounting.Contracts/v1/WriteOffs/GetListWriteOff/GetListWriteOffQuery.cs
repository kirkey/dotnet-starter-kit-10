using FSH.Module.Accounting.Contracts.v1.WriteOffs;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.WriteOffs.GetListWriteOff;

public record GetListWriteOffQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<WriteOffsPagedResponse>;

public record WriteOffsPagedResponse(
    List<WriteOffSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);
