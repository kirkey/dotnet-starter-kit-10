using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Accruals.CreateAccrual;

public record CreateAccrualCommand(string Name, string? Description) : ICommand<Guid>;