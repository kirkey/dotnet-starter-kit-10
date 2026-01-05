using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Accruals.UpdateAccrual;

public record UpdateAccrualCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;