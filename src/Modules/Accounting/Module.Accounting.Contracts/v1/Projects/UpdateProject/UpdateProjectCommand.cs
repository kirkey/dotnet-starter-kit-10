using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Projects.UpdateProject;

public record UpdateProjectCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;