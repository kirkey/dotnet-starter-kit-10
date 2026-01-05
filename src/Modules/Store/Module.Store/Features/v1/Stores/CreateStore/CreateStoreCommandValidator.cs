using FluentValidation;
using FSH.Module.Store.Contracts.v1.Stores;

namespace FSH.Module.Store.Features.v1.Stores.CreateStore;

public class CreateStoreCommandValidator : AbstractValidator<CreateStoreCommand>
{
    public CreateStoreCommandValidator()
    {
        RuleFor(x => x.Name).ValidateStoreName();
        RuleFor(x => x.Address).ValidateStoreAddress();
        RuleFor(x => x.City).NotEmpty().MaximumLength(StoreStringLengths.StoreCityMaxLength);
        RuleFor(x => x.PostalCode).NotEmpty().MaximumLength(StoreStringLengths.StorePostalCodeMaxLength);
    }
}
