using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.WriteOffs.ReverseWriteOff;

public record ReverseWriteOffCommand(Guid Id) : ICommand;