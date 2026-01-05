using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.StaffTrainings.CreateStaffTraining;

public sealed record CreateStaffTrainingCommand(string Name) : ICommand<Guid>;
