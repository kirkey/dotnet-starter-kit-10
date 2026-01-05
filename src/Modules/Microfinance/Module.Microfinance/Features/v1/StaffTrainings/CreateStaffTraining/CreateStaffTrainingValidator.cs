using FSH.Module.Microfinance.Contracts.v1.StaffTrainings.CreateStaffTraining;

namespace FSH.Module.Microfinance.Features.v1.StaffTrainings.CreateStaffTraining;

public class CreateStaffTrainingValidator : AbstractValidator<CreateStaffTrainingCommand>
{
    public CreateStaffTrainingValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
