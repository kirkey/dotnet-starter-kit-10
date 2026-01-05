using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.WriteOffs.GetListWriteOff;

public sealed record GetWriteOffsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<WriteOffsPagedResponse>;
