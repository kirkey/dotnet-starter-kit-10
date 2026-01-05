using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Members.DeleteMember;

public record DeleteMemberCommand(Guid Id) : ICommand;