using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.WriteOffs.CreateWriteOff;

public record CreateWriteOffCommand(string Name, string? Description) : ICommand<Guid>;
