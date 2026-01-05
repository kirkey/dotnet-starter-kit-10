using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Projects.CreateProject;

public record CreateProjectCommand(string Name, string? Description) : ICommand<Guid>;