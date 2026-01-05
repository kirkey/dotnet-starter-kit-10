namespace FSH.Module.Catalog;

/// <summary>
/// Defines all string length constants for the Catalog module.
/// 
/// **Purpose:**
/// Centralizes all string field length constraints using power-of-2 values (4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, ...).
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
/// Example: CategoryNameMaxLength, ProductDescriptionMaxLength
/// 
/// **Why Power of 2?**
/// - Powers of 2 align with memory boundaries
/// - More efficient for string algorithms
/// - Better database index performance
/// - Industry standard for string constraints
/// - Easier to scale and extend
/// 
/// **String Length Mapping:**
/// - Identifiers/IDs: 64 chars (GUIDs, User IDs, Tenant IDs)
/// - Names/Titles: 128 chars (Category names, Brand names, Product names)
/// - Descriptions: 512 chars (Detailed descriptions)
/// - SKU/Code: 64 chars (Product codes, SKUs)
/// - Large text: 2048 chars (Product details, specifications)
/// - Usernames/Email parts: 256 chars (Created/Modified by usernames)
/// </summary>
public static class CatalogStringLengths
{
    // ========== Category String Lengths ==========
    
    /// <summary>
    /// Category entity name field maximum length.
    /// 
    /// **Usage:** Category.Name property
    /// **Validator:** CreateCategoryCommandValidator, UpdateCategoryCommandValidator
    /// **Database:** CategoryConfiguration.Name column
    /// 
    /// **Rationale:** 128 chars allows for descriptive category names
    /// Examples: "Electronics", "Home & Garden", "Sports & Outdoors"
    /// </summary>
    public const int CategoryNameMaxLength = 128;
    
    /// <summary>
    /// Category entity description field maximum length.
    /// 
    /// **Usage:** Category.Description property
    /// **Validator:** CreateCategoryCommandValidator, UpdateCategoryCommandValidator
    /// **Database:** CategoryConfiguration.Description column
    /// 
    /// **Rationale:** 512 chars provides room for detailed category descriptions
    /// Examples: Detailed explanations of what products belong in the category
    /// </summary>
    public const int CategoryDescriptionMaxLength = 512;
    
    /// <summary>
    /// Category entity code field maximum length.
    /// 
    /// **Usage:** Category.Code property
    /// **Database:** CategoryConfiguration.Code column
    /// 
    /// **Rationale:** 64 chars for unique category codes/identifiers
    /// Examples: "ELEC-001", "HOME-GARDEN", custom category codes
    /// </summary>
    public const int CategoryCodeMaxLength = 64;
    
    /// <summary>
    /// Category entity tenant ID field maximum length.
    /// 
    /// **Usage:** Category.TenantId property
    /// **Database:** CategoryConfiguration.TenantId column
    /// 
    /// **Rationale:** 64 chars supports various tenant ID formats
    /// </summary>
    public const int CategoryTenantIdMaxLength = 64;
    
    /// <summary>
    /// Category entity created by username field maximum length.
    /// 
    /// **Usage:** Category.CreatedByUserName property
    /// **Database:** CategoryConfiguration.CreatedByUserName column
    /// 
    /// **Rationale:** 256 chars accommodates usernames and email addresses
    /// </summary>
    public const int CategoryCreatedByUserNameMaxLength = 256;
    
    /// <summary>
    /// Category entity last modified by username field maximum length.
    /// 
    /// **Usage:** Category.LastModifiedByUserName property
    /// **Database:** CategoryConfiguration.LastModifiedByUserName column
    /// 
    /// **Rationale:** 256 chars for consistency
    /// </summary>
    public const int CategoryLastModifiedByUserNameMaxLength = 256;
    
    // ========== Brand String Lengths ==========
    
    /// <summary>
    /// Brand entity name field maximum length.
    /// 
    /// **Usage:** Brand.Name property
    /// **Validator:** CreateBrandCommandValidator, UpdateBrandCommandValidator
    /// **Database:** BrandConfiguration.Name column
    /// 
    /// **Rationale:** 128 chars allows for brand names and full company names
    /// Examples: "Apple Inc.", "Samsung Electronics", "Procter & Gamble"
    /// </summary>
    public const int BrandNameMaxLength = 128;
    
    /// <summary>
    /// Brand entity description field maximum length.
    /// 
    /// **Usage:** Brand.Description property
    /// **Validator:** CreateBrandCommandValidator, UpdateBrandCommandValidator
    /// **Database:** BrandConfiguration.Description column
    /// 
    /// **Rationale:** 512 chars provides room for brand descriptions
    /// Examples: Brand history, values, mission statements
    /// </summary>
    public const int BrandDescriptionMaxLength = 512;
    
    /// <summary>
    /// Brand entity website URL field maximum length.
    /// 
    /// **Usage:** Brand.WebsiteUrl property
    /// **Database:** BrandConfiguration.WebsiteUrl column
    /// 
    /// **Rationale:** 256 chars accommodates full URLs
    /// Examples: "https://www.brand-name.com/about-us"
    /// </summary>
    public const int BrandWebsiteUrlMaxLength = 256;
    
    /// <summary>
    /// Brand entity tenant ID field maximum length.
    /// 
    /// **Usage:** Brand.TenantId property
    /// **Database:** BrandConfiguration.TenantId column
    /// 
    /// **Rationale:** 64 chars supports various tenant ID formats
    /// </summary>
    public const int BrandTenantIdMaxLength = 64;
    
    /// <summary>
    /// Brand entity created by username field maximum length.
    /// 
    /// **Usage:** Brand.CreatedByUserName property
    /// **Database:** BrandConfiguration.CreatedByUserName column
    /// 
    /// **Rationale:** 256 chars accommodates usernames and email addresses
    /// </summary>
    public const int BrandCreatedByUserNameMaxLength = 256;
    
    /// <summary>
    /// Brand entity last modified by username field maximum length.
    /// 
    /// **Usage:** Brand.LastModifiedByUserName property
    /// **Database:** BrandConfiguration.LastModifiedByUserName column
    /// 
    /// **Rationale:** 256 chars for consistency
    /// </summary>
    public const int BrandLastModifiedByUserNameMaxLength = 256;
    
    // ========== Product String Lengths ==========
    
    /// <summary>
    /// Product entity name field maximum length.
    /// 
    /// **Usage:** Product.Name property
    /// **Validator:** CreateProductCommandValidator, UpdateProductCommandValidator
    /// **Database:** ProductConfiguration.Name column
    /// 
    /// **Rationale:** 256 chars allows for full product names with details
    /// Examples: "Samsung Galaxy S24 Ultra 512GB 5G Smartphone - Titanium Gray"
    /// </summary>
    public const int ProductNameMaxLength = 256;
    
    /// <summary>
    /// Product entity description field maximum length.
    /// 
    /// **Usage:** Product.Description property
    /// **Validator:** CreateProductCommandValidator, UpdateProductCommandValidator
    /// **Database:** ProductConfiguration.Description column
    /// 
    /// **Rationale:** 1024 chars for detailed product descriptions
    /// Examples: Product features, specifications, included items
    /// </summary>
    public const int ProductDescriptionMaxLength = 1024;
    
    /// <summary>
    /// Product entity SKU field maximum length.
    /// 
    /// **Usage:** Product.SKU property
    /// **Validator:** CreateProductCommandValidator, UpdateProductCommandValidator
    /// **Database:** ProductConfiguration.SKU column
    /// 
    /// **Rationale:** 64 chars for SKU codes and product identifiers
    /// Examples: "SAMS24U-512-TG", "PROD-12345-ABC"
    /// </summary>
    public const int ProductSKUMaxLength = 64;
    
    /// <summary>
    /// Product entity barcode field maximum length.
    /// 
    /// **Usage:** Product.Barcode property
    /// **Database:** ProductConfiguration.Barcode column
    /// 
    /// **Rationale:** 128 chars accommodates various barcode formats
    /// Examples: UPC (12), EAN-13 (13), Code 128, QR codes
    /// </summary>
    public const int ProductBarcodeMaxLength = 128;
    
    /// <summary>
    /// Product entity specifications field maximum length.
    /// 
    /// **Usage:** Product.Specifications property
    /// **Database:** ProductConfiguration.Specifications column
    /// 
    /// **Rationale:** 2048 chars for detailed technical specifications
    /// Examples: Dimensions, weight, materials, technical details
    /// </summary>
    public const int ProductSpecificationsMaxLength = 2048;
    
    /// <summary>
    /// Product entity tenant ID field maximum length.
    /// 
    /// **Usage:** Product.TenantId property
    /// **Database:** ProductConfiguration.TenantId column
    /// 
    /// **Rationale:** 64 chars supports various tenant ID formats
    /// </summary>
    public const int ProductTenantIdMaxLength = 64;
    
    /// <summary>
    /// Product entity created by username field maximum length.
    /// 
    /// **Usage:** Product.CreatedByUserName property
    /// **Database:** ProductConfiguration.CreatedByUserName column
    /// 
    /// **Rationale:** 256 chars accommodates usernames and email addresses
    /// </summary>
    public const int ProductCreatedByUserNameMaxLength = 256;
    
    /// <summary>
    /// Product entity last modified by username field maximum length.
    /// 
    /// **Usage:** Product.LastModifiedByUserName property
    /// **Database:** ProductConfiguration.LastModifiedByUserName column
    /// 
    /// **Rationale:** 256 chars for consistency
    /// </summary>
    public const int ProductLastModifiedByUserNameMaxLength = 256;
    
    // ========== Common Constants (Shared) ==========
    
    /// <summary>
    /// Standard maximum length for email addresses and usernames.
    /// 
    /// **Usage:** User identity properties, email-like identifiers
    /// **Rationale:** 256 chars accommodates full email addresses with extensions
    /// </summary>
    public const int StandardUsernameMaxLength = 256;
    
    /// <summary>
    /// Standard maximum length for tenant/organization identifiers.
    /// 
    /// **Usage:** Tenant IDs, organization identifiers
    /// **Rationale:** 64 chars supports GUIDs, UUIDs, and custom formats
    /// </summary>
    public const int StandardTenantIdMaxLength = 64;
}
