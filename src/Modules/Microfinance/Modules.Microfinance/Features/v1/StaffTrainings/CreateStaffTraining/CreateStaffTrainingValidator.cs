namespace FSH.Modules.Microfinance.Features.v1.StaffTrainings.CreateStaffTraining;

public class CreateStaffTrainingValidator : AbstractValidator<CreateStaffTrainingCommand>
{
    public CreateStaffTrainingValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
