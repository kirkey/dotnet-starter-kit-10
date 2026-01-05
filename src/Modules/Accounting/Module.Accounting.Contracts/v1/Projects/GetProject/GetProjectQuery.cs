using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Projects.GetProject;

public record GetProjectQuery(Guid Id) : IQuery<ProjectDto>;