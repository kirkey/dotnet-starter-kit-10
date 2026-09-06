using FSH.Modules.Ai.Contracts.Dtos;
using Mediator;

namespace FSH.Modules.Ai.Contracts.v1.Departments;

public sealed record ListDepartmentsQuery : IQuery<IReadOnlyList<AiDepartmentDto>>;
