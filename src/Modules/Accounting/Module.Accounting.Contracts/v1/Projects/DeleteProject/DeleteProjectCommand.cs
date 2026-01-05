using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Projects.DeleteProject;

public record DeleteProjectCommand(Guid Id) : ICommand;