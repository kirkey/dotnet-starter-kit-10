using FluentValidation;

namespace FSH.Module.Catalog.Features;

/// <summary>
/// Extension methods for FluentValidation rules used across Catalog features.
/// 
/// **Purpose:**
/// Centralizes common validation rules to avoid duplication across multiple validators.
/// All rules reference property-specific string length constants for consistency.
/// </summary>
public static class CatalogValidationExtensions
{
    /// <summary>Validates category name: Required, max length.</summary>
    public static IRuleBuilderOptions<T, string> ValidateCategoryName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Category name is required")
            .MaximumLength(CatalogStringLengths.CategoryNameMaxLength)
            .WithMessage($"Category name cannot exceed {CatalogStringLengths.CategoryNameMaxLength} characters");
    }
    
    /// <summary>Validates category code: Required, max length.</summary>
    public static IRuleBuilderOptions<T, string> ValidateCategoryCode<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Category code is required")
            .MaximumLength(CatalogStringLengths.CategoryCodeMaxLength)
            .WithMessage($"Category code cannot exceed {CatalogStringLengths.CategoryCodeMaxLength} characters");
    }
    
    /// <summary>Validates category description: Optional, max length.</summary>
    public static IRuleBuilderOptions<T, string?> ValidateCategoryDescription<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(CatalogStringLengths.CategoryDescriptionMaxLength)
            .WithMessage($"Description cannot exceed {CatalogStringLengths.CategoryDescriptionMaxLength} characters");
    }
    
    /// <summary>Validates brand name: Required, max length.</summary>
    public static IRuleBuilderOptions<T, string> ValidateBrandName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Brand name is required")
            .MaximumLength(CatalogStringLengths.BrandNameMaxLength)
            .WithMessage($"Brand name cannot exceed {CatalogStringLengths.BrandNameMaxLength} characters");
    }
    
    /// <summary>Validates brand description: Optional, max length.</summary>
    public static IRuleBuilderOptions<T, string?> ValidateBrandDescription<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(CatalogStringLengths.BrandDescriptionMaxLength)
            .WithMessage($"Description cannot exceed {CatalogStringLengths.BrandDescriptionMaxLength} characters");
    }
    
    /// <summary>Validates product name: Required, max length.</summary>
    public static IRuleBuilderOptions<T, string> ValidateProductName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(CatalogStringLengths.ProductNameMaxLength)
            .WithMessage($"Product name cannot exceed {CatalogStringLengths.ProductNameMaxLength} characters");
    }
    
    /// <summary>Validates product SKU: Required, max length.</summary>
    public static IRuleBuilderOptions<T, string> ValidateProductSKU<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Product SKU is required")
            .MaximumLength(CatalogStringLengths.ProductSKUMaxLength)
            .WithMessage($"SKU cannot exceed {CatalogStringLengths.ProductSKUMaxLength} characters");
    }
    
    /// <summary>Validates product description: Optional, max length.</summary>
    public static IRuleBuilderOptions<T, string?> ValidateProductDescription<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(CatalogStringLengths.ProductDescriptionMaxLength)
            .WithMessage($"Description cannot exceed {CatalogStringLengths.ProductDescriptionMaxLength} characters");
    }
}
