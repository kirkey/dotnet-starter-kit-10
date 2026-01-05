using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.StaffTrainings.GetStaffTraining;

public sealed record GetStaffTrainingQuery(Guid Id) : IQuery<StaffTrainingDto>;
