namespace Accounting.Application.WriteOffs.Update.v1;

public sealed record UpdateWriteOffCommand(
    DefaultIdType Id,
    string? Reason,
    string? Description,
    string? Notes
) : IRequest<DefaultIdType>;
