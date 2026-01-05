using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Members.UpdateMember;

public record UpdateMemberCommand(Guid Id, string Name, string? Description, bool IsActive) : ICommand<Guid>;