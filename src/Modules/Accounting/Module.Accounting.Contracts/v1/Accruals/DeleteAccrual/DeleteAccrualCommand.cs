using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Accruals.DeleteAccrual;

public record DeleteAccrualCommand(Guid Id) : ICommand;