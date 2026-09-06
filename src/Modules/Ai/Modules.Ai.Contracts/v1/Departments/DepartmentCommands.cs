using Mediator;

namespace FSH.Modules.Ai.Contracts.v1.Departments;

public sealed record CreateDepartmentCommand(string Name, string? Description) : ICommand<Guid>;

public sealed record UpdateDepartmentCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

public sealed record DeleteDepartmentCommand(Guid Id) : ICommand<Guid>;
