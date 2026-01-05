namespace FSH.Module.Catalog;

/// <summary>
/// Defines all string length constants for the Catalog module.
/// 
/// **Purpose:**
/// Centralizes all string field length constraints using power-of-2 values (4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048).
/// This ensures consistency across domain entities, validators, and database configurations.
/// 
/// **Design Pattern:**
/// - Uses binary-friendly sizes (powers of 2) for memory efficiency and alignment
/// - Single source of truth for all string constraints
/// - Easy to maintain and refactor
/// - Clear naming conventions indicate usage context
/// 
/// **Naming Convention:**
/// {EntityName}{PropertyName}MaxLength
/// Example: CategoryNameMaxLength, ProductNameMaxLength
/// </summary>
public static class CatalogStringLengths
{
    // ========== Category String Lengths ==========
    
    /// <summary>
    /// Category entity name field maximum length.
    /// 
    /// **Usage:** Category.Name property
    /// **Rationale:** 128 chars allows for descriptive category names
    /// </summary>
    public const int CategoryNameMaxLength = 128;
    
    /// <summary>
    /// Category entity description field maximum length.
    /// 
    /// **Usage:** Category.Description property
    /// **Rationale:** 512 chars provides room for detailed descriptions
    /// </summary>
    public const int CategoryDescriptionMaxLength = 512;
    
    /// <summary>
    /// Category entity code field maximum length.
    /// 
    /// **Usage:** Category.Code property (unique identifier)
    /// **Rationale:** 64 chars for category codes/SKU prefixes
    /// </summary>
    public const int CategoryCodeMaxLength = 64;
    
    /// <summary>
    /// Category entity tenant ID field maximum length.
    /// 
    /// **Usage:** Category.TenantId property
    /// **Rationale:** 64 chars supports various tenant ID formats
    /// </summary>
    public const int CategoryTenantIdMaxLength = 64;
    
    /// <summary>
    /// Category entity created by username field maximum length.
    /// 
    /// **Usage:** Category.CreatedByUserName property
    /// **Rationale:** 256 chars accommodates usernames and email addresses
    /// </summary>
    public const int CategoryCreatedByUserNameMaxLength = 256;
    
    /// <summary>
    /// Category entity last modified by username field maximum length.
    /// 
    /// **Usage:** Category.LastModifiedByUserName property
    /// **Rationale:** 256 chars for consistency
    /// </summary>
    public const int CategoryLastModifiedByUserNameMaxLength = 256;
    
    // ========== Brand String Lengths ==========
    
    /// <summary>
    /// Brand entity name field maximum length.
    /// 
    /// **Usage:** Brand.Name property
    /// **Rationale:** 128 chars allows for brand names
    /// </summary>
    public const int BrandNameMaxLength = 128;
    
    /// <summary>
    /// Brand entity description field maximum length.
    /// 
    /// **Usage:** Brand.Description property
    /// **Rationale:** 512 chars for brand descriptions
    /// </summary>
    public const int BrandDescriptionMaxLength = 512;
    
    /// <summary>
    /// Brand entity website URL field maximum length.
    /// 
    /// **Usage:** Brand.Website property
    /// **Rationale:** 512 chars accommodates long URLs
    /// </summary>
    public const int BrandWebsiteMaxLength = 512;
    
    /// <summary>
    /// Brand entity tenant ID field maximum length.
    /// 
    /// **Usage:** Brand.TenantId property
    /// **Rationale:** 64 chars supports various tenant ID formats
    /// </summary>
    public const int BrandTenantIdMaxLength = 64;
    
    /// <summary>
    /// Brand entity created by username field maximum length.
    /// 
    /// **Usage:** Brand.CreatedByUserName property
    /// **Rationale:** 256 chars accommodates usernames and email addresses
    /// </summary>
    public const int BrandCreatedByUserNameMaxLength = 256;
    
    /// <summary>
    /// Brand entity last modified by username field maximum length.
    /// 
    /// **Usage:** Brand.LastModifiedByUserName property
    /// **Rationale:** 256 chars for consistency
    /// </summary>
    public const int BrandLastModifiedByUserNameMaxLength = 256;
    
    // ========== Product String Lengths ==========
    
    /// <summary>
    /// Product entity name field maximum length.
    /// 
    /// **Usage:** Product.Name property
    /// **Rationale:** 256 chars allows for descriptive product names
    /// </summary>
    public const int ProductNameMaxLength = 256;
    
    /// <summary>
    /// Product entity description field maximum length.
    /// 
    /// **Usage:** Product.Description property
    /// **Rationale:** 2048 chars provides extensive product descriptions
    /// </summary>
    public const int ProductDescriptionMaxLength = 2048;
    
    /// <summary>
    /// Product entity SKU field maximum length.
    /// 
    /// **Usage:** Product.SKU property (Stock Keeping Unit)
    /// **Rationale:** 64 chars for product SKUs
    /// </summary>
    public const int ProductSKUMaxLength = 64;
    
    /// <summary>
    /// Product entity barcode field maximum length.
    /// 
    /// **Usage:** Product.Barcode property
    /// **Rationale:** 128 chars supports various barcode formats
    /// </summary>
    public const int ProductBarcodeMaxLength = 128;
    
    /// <summary>
    /// Product entity status field maximum length.
    /// 
    /// **Usage:** Product.Status property (enum string conversion)
    /// **Rationale:** 32 chars is sufficient for all status values
    /// </summary>
    public const int ProductStatusMaxLength = 32;
    
    /// <summary>
    /// Product entity tenant ID field maximum length.
    /// 
    /// **Usage:** Product.TenantId property
    /// **Rationale:** 64 chars supports various tenant ID formats
    /// </summary>
    public const int ProductTenantIdMaxLength = 64;
    
    /// <summary>
    /// Product entity created by username field maximum length.
    /// 
    /// **Usage:** Product.CreatedByUserName property
    /// **Rationale:** 256 chars accommodates usernames and email addresses
    /// </summary>
    public const int ProductCreatedByUserNameMaxLength = 256;
    
    /// <summary>
    /// Product entity last modified by username field maximum length.
    /// 
    /// **Usage:** Product.LastModifiedByUserName property
    /// **Rationale:** 256 chars for consistency
    /// </summary>
    public const int ProductLastModifiedByUserNameMaxLength = 256;
    
    // ========== Common Constants (Shared) ==========
    
    /// <summary>
    /// Standard maximum length for email addresses and usernames.
    /// </summary>
    public const int StandardUsernameMaxLength = 256;
    
    /// <summary>
    /// Standard maximum length for tenant/organization identifiers.
    /// </summary>
    public const int StandardTenantIdMaxLength = 64;
    
    /// <summary>
    /// Standard maximum length for status enum string representations.
    /// </summary>
    public const int StandardStatusMaxLength = 32;
}
