using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.StaffTrainings.UpdateStaffTraining;

public sealed record UpdateStaffTrainingCommand(Guid Id, string Name) : ICommand<Guid>;
