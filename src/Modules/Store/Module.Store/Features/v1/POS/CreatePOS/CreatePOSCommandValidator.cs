using FluentValidation;
using FSH.Module.Store.Contracts.v1.POS;

namespace FSH.Module.Store.Features.v1.POS.CreatePOS;

public class CreatePOSCommandValidator : AbstractValidator<CreatePOSCommand>
{
    public CreatePOSCommandValidator()
    {
        RuleFor(x => x.Name).ValidatePOSName();
        RuleFor(x => x.Identifier).ValidatePOSIdentifier();
        RuleFor(x => x.StoreId).NotEmpty().WithMessage("Store is required");
    }
}
