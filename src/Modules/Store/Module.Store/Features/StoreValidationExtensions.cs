using FluentValidation;

namespace FSH.Module.Store.Features;

public static class StoreValidationExtensions
{
    public static IRuleBuilderOptions<T, string> ValidateStoreName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Store name is required")
            .MaximumLength(StoreStringLengths.StoreNameMaxLength);
    }
    
    public static IRuleBuilderOptions<T, string> ValidateStoreAddress<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(StoreStringLengths.StoreAddressMaxLength);
    }
    
    public static IRuleBuilderOptions<T, string> ValidatePOSName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("POS name is required")
            .MaximumLength(StoreStringLengths.POSNameMaxLength);
    }
    
    public static IRuleBuilderOptions<T, string> ValidatePOSIdentifier<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("POS identifier is required")
            .MaximumLength(StoreStringLengths.POSIdentifierMaxLength);
    }
}
