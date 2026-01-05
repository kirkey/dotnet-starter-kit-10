using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Members.CreateMember;

public record CreateMemberCommand(string Name, string? Description, bool IsActive = true) : ICommand<Guid>;