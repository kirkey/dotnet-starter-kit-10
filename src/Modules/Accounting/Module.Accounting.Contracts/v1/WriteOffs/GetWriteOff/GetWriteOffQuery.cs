using FSH.Module.Accounting.Contracts.v1.WriteOffs;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.WriteOffs.GetWriteOff;

public record GetWriteOffQuery(Guid Id) : IQuery<WriteOffDto>;
