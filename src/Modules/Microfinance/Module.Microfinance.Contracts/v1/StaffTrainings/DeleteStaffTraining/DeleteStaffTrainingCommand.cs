using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.StaffTrainings.DeleteStaffTraining;

public sealed record DeleteStaffTrainingCommand(Guid Id) : ICommand;
