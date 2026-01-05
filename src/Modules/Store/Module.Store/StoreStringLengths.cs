namespace FSH.Module.Store;

/// <summary>
/// Defines all string length constants for the Store module.
/// 
/// Uses power-of-2 values for optimal performance.
/// </summary>
public static class StoreStringLengths
{
    // ========== Store String Lengths ==========
    
    /// <summary>Store name maximum length (128 chars).</summary>
    public const int StoreNameMaxLength = 128;
    
    /// <summary>Store description maximum length (512 chars).</summary>
    public const int StoreDescriptionMaxLength = 512;
    
    /// <summary>Store address maximum length (256 chars).</summary>
    public const int StoreAddressMaxLength = 256;
    
    /// <summary>Store city maximum length (128 chars).</summary>
    public const int StoreCityMaxLength = 128;
    
    /// <summary>Store state/province maximum length (64 chars).</summary>
    public const int StoreStateMaxLength = 64;
    
    /// <summary>Store postal code maximum length (32 chars).</summary>
    public const int StorePostalCodeMaxLength = 32;
    
    /// <summary>Store phone number maximum length (32 chars).</summary>
    public const int StorePhoneMaxLength = 32;
    
    /// <summary>Store email maximum length (256 chars).</summary>
    public const int StoreEmailMaxLength = 256;
    
    /// <summary>Store tenant ID maximum length (64 chars).</summary>
    public const int StoreTenantIdMaxLength = 64;
    
    /// <summary>Store created by username maximum length (256 chars).</summary>
    public const int StoreCreatedByUserNameMaxLength = 256;
    
    /// <summary>Store last modified by username maximum length (256 chars).</summary>
    public const int StoreLastModifiedByUserNameMaxLength = 256;
    
    // ========== POS String Lengths ==========
    
    /// <summary>POS name maximum length (128 chars).</summary>
    public const int POSNameMaxLength = 128;
    
    /// <summary>POS description maximum length (512 chars).</summary>
    public const int POSDescriptionMaxLength = 512;
    
    /// <summary>POS identifier/code maximum length (64 chars).</summary>
    public const int POSIdentifierMaxLength = 64;
    
    /// <summary>POS location within store maximum length (128 chars).</summary>
    public const int POSLocationMaxLength = 128;
    
    /// <summary>POS tenant ID maximum length (64 chars).</summary>
    public const int POSTenantIdMaxLength = 64;
    
    /// <summary>POS created by username maximum length (256 chars).</summary>
    public const int POSCreatedByUserNameMaxLength = 256;
    
    /// <summary>POS last modified by username maximum length (256 chars).</summary>
    public const int POSLastModifiedByUserNameMaxLength = 256;
}
