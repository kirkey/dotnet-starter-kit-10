using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.WriteOffs.ApproveWriteOff;

public record ApproveWriteOffCommand(Guid Id) : ICommand;