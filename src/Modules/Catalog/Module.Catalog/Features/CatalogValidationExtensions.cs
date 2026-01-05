using FluentValidation;

namespace FSH.Module.Catalog.Features;

/// <summary>
/// Extension methods for FluentValidation rules used across Catalog features.
/// 
/// **Purpose:**
/// Centralizes common validation rules to avoid duplication across multiple validators.
/// All rules reference property-specific string length constants for consistency.
/// 
/// **Benefits:**
/// - DRY principle: Define once, use everywhere
/// - Consistency: All validators use the same rules
/// - Maintainability: Update one place, applies everywhere
/// - Reusability: Easily compose validators
/// </summary>
public static class CatalogValidationExtensions
{
    // ===== Category Validations =====
    
    /// <summary>
    /// Validates category name: Required, max length.
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidateCategoryName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Category name is required")
            .MaximumLength(CatalogStringLengths.CategoryNameMaxLength)
            .WithMessage($"Category name cannot exceed {CatalogStringLengths.CategoryNameMaxLength} characters");
    }
    
    /// <summary>
    /// Validates category code: Required, max length.
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidateCategoryCode<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Category code is required")
            .MaximumLength(CatalogStringLengths.CategoryCodeMaxLength)
            .WithMessage($"Category code cannot exceed {CatalogStringLengths.CategoryCodeMaxLength} characters");
    }
    
    /// <summary>
    /// Validates category description: Optional, max length.
    /// </summary>
    public static IRuleBuilderOptions<T, string?> ValidateCategoryDescription<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(CatalogStringLengths.CategoryDescriptionMaxLength)
            .WithMessage($"Description cannot exceed {CatalogStringLengths.CategoryDescriptionMaxLength} characters");
    }
    
    // ===== Brand Validations =====
    
    /// <summary>
    /// Validates brand name: Required, max length.
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidateBrandName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Brand name is required")
            .MaximumLength(CatalogStringLengths.BrandNameMaxLength)
            .WithMessage($"Brand name cannot exceed {CatalogStringLengths.BrandNameMaxLength} characters");
    }
    
    /// <summary>
    /// Validates brand description: Optional, max length.
    /// </summary>
    public static IRuleBuilderOptions<T, string?> ValidateBrandDescription<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(CatalogStringLengths.BrandDescriptionMaxLength)
            .WithMessage($"Description cannot exceed {CatalogStringLengths.BrandDescriptionMaxLength} characters");
    }
    
    /// <summary>
    /// Validates brand website URL: Optional, max length.
    /// </summary>
    public static IRuleBuilderOptions<T, string?> ValidateBrandWebsiteUrl<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(CatalogStringLengths.BrandWebsiteUrlMaxLength)
            .WithMessage($"Website URL cannot exceed {CatalogStringLengths.BrandWebsiteUrlMaxLength} characters");
    }
    
    // ===== Product Validations =====
    
    /// <summary>
    /// Validates product name: Required, max length.
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidateProductName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(CatalogStringLengths.ProductNameMaxLength)
            .WithMessage($"Product name cannot exceed {CatalogStringLengths.ProductNameMaxLength} characters");
    }
    
    /// <summary>
    /// Validates product SKU: Required, max length.
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidateProductSKU<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Product SKU is required")
            .MaximumLength(CatalogStringLengths.ProductSKUMaxLength)
            .WithMessage($"Product SKU cannot exceed {CatalogStringLengths.ProductSKUMaxLength} characters");
    }
    
    /// <summary>
    /// Validates product description: Optional, max length.
    /// </summary>
    public static IRuleBuilderOptions<T, string?> ValidateProductDescription<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(CatalogStringLengths.ProductDescriptionMaxLength)
            .WithMessage($"Description cannot exceed {CatalogStringLengths.ProductDescriptionMaxLength} characters");
    }
    
    /// <summary>
    /// Validates product barcode: Optional, max length.
    /// </summary>
    public static IRuleBuilderOptions<T, string?> ValidateProductBarcode<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(CatalogStringLengths.ProductBarcodeMaxLength)
            .WithMessage($"Barcode cannot exceed {CatalogStringLengths.ProductBarcodeMaxLength} characters");
    }
    
    /// <summary>
    /// Validates product specifications: Optional, max length.
    /// </summary>
    public static IRuleBuilderOptions<T, string?> ValidateProductSpecifications<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(CatalogStringLengths.ProductSpecificationsMaxLength)
            .WithMessage($"Specifications cannot exceed {CatalogStringLengths.ProductSpecificationsMaxLength} characters");
    }
    
    /// <summary>
    /// Validates product price: Required, non-negative.
    /// </summary>
    public static IRuleBuilderOptions<T, decimal> ValidateProductPrice<T>(
        this IRuleBuilder<T, decimal> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThanOrEqualTo(0).WithMessage("Price cannot be negative");
    }
    
    /// <summary>
    /// Validates product cost: Optional, non-negative.
    /// </summary>
    public static IRuleBuilderOptions<T, decimal?> ValidateProductCost<T>(
        this IRuleBuilder<T, decimal?> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThanOrEqualTo(0).WithMessage("Cost cannot be negative");
    }
    
    /// <summary>
    /// Validates product quantity: Required, non-negative.
    /// </summary>
    public static IRuleBuilderOptions<T, int> ValidateProductQuantity<T>(
        this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative");
    }
}
