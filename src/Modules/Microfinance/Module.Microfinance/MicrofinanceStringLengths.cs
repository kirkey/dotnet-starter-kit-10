namespace FSH.Module.Microfinance;

/// <summary>
/// Defines all string length constants for the Microfinance module using power-of-2 values.
/// 
/// **Purpose:**
/// Centralizes all string field length constraints ensuring consistency across domain entities,
/// validators, and database configurations. Uses binary-friendly sizes for optimal performance.
/// 
/// **Design Pattern:**
/// - Power-of-2 values (4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048)
/// - Single source of truth for all string constraints
/// - Memory-efficient and database-optimized
/// 
/// **Naming Convention:**
/// {EntityName}{PropertyName}MaxLength or {Purpose}MaxLength
/// 
/// **Standard Sizes:**
/// - XSmall (4): Very short codes/flags
/// - Small (8): Status codes, short identifiers
/// - Regular (16): Account codes, short reference numbers
/// - Medium (32): Short names, types, status values
/// - Large (64): IDs, member numbers, reference numbers
/// - XLarge (128): Names, titles
/// - XXLarge (256): Classifications, longer names, email addresses
/// - Huge (512): Descriptions, addresses
/// - XHuge (1024): Detailed descriptions
/// - XXHuge (2048): Large text content, notes
/// </summary>
public static class MicrofinanceStringLengths
{
    // ========================================
    // Common Sizes (Reusable across entities)
    // ========================================
    
    /// <summary>XSmall: 4 characters - Very short codes, flags</summary>
    public const int XSmall = 4;
    
    /// <summary>Small: 8 characters - Status codes, currency codes</summary>
    public const int Small = 8;
    
    /// <summary>Regular: 16 characters - Short reference codes</summary>
    public const int Regular = 16;
    
    /// <summary>Medium: 32 characters - Types, categories, short names, gender</summary>
    public const int Medium = 32;
    
    /// <summary>Large: 64 characters - IDs, member numbers, standard identifiers</summary>
    public const int Large = 64;
    
    /// <summary>XLarge: 128 characters - Names, titles</summary>
    public const int XLarge = 128;
    
    /// <summary>XXLarge: 256 characters - Classifications, longer names, emails, occupations</summary>
    public const int XXLarge = 256;
    
    /// <summary>Huge: 512 characters - Descriptions, addresses, notes</summary>
    public const int Huge = 512;
    
    /// <summary>XHuge: 1024 characters - Detailed descriptions, content</summary>
    public const int XHuge = 1024;
    
    /// <summary>XXHuge: 2048 characters - Large text, detailed notes</summary>
    public const int XXHuge = 2048;

    // ========================================
    // Member (Customer/Client)
    // ========================================
    
    /// <summary>Member number/ID</summary>
    public const int MemberNumber = Large;
    
    /// <summary>Member first name</summary>
    public const int MemberFirstName = XLarge;
    
    /// <summary>Member last name</summary>
    public const int MemberLastName = XLarge;
    
    /// <summary>Member middle name</summary>
    public const int MemberMiddleName = XLarge;
    
    /// <summary>Member email address</summary>
    public const int MemberEmail = XXLarge;
    
    /// <summary>Member phone number</summary>
    public const int MemberPhoneNumber = Medium;
    
    /// <summary>Member address</summary>
    public const int MemberAddress = Huge;
    
    /// <summary>Member national ID</summary>
    public const int MemberNationalId = Large;
    
    /// <summary>Member occupation</summary>
    public const int MemberOccupation = XXLarge;
    
    /// <summary>Member gender</summary>
    public const int MemberGender = Medium;
    
    /// <summary>Minimum length for first name</summary>
    public const int MemberFirstNameMinLength = 2;
    
    /// <summary>Minimum length for last name</summary>
    public const int MemberLastNameMinLength = 2;

    // ========================================
    // Loan
    // ========================================
    
    /// <summary>Loan name/title</summary>
    public const int LoanName = XXLarge;
    
    /// <summary>Loan number/reference</summary>
    public const int LoanNumber = Large;
    
    /// <summary>Loan status</summary>
    public const int LoanStatus = Medium;
    
    /// <summary>Loan type</summary>
    public const int LoanType = XLarge;
    
    /// <summary>Loan description</summary>
    public const int LoanDescription = Huge;
    
    /// <summary>Loan purpose</summary>
    public const int LoanPurpose = Huge;
    
    /// <summary>Loan notes</summary>
    public const int LoanNotes = XXHuge;

    // ========================================
    // Savings Account
    // ========================================
    
    /// <summary>Savings account name</summary>
    public const int SavingsAccountName = XXLarge;
    
    /// <summary>Savings account number</summary>
    public const int SavingsAccountNumber = Large;
    
    /// <summary>Savings account status</summary>
    public const int SavingsAccountStatus = Medium;
    
    /// <summary>Savings account type</summary>
    public const int SavingsAccountType = XLarge;
    
    /// <summary>Savings account description</summary>
    public const int SavingsAccountDescription = Huge;

    // ========================================
    // Loan Application
    // ========================================
    
    /// <summary>Loan application name/title</summary>
    public const int LoanApplicationName = XXLarge;
    
    /// <summary>Loan application number</summary>
    public const int LoanApplicationNumber = Large;
    
    /// <summary>Loan application status</summary>
    public const int LoanApplicationStatus = Medium;
    
    /// <summary>Loan application purpose</summary>
    public const int LoanApplicationPurpose = Huge;
    
    /// <summary>Loan application notes</summary>
    public const int LoanApplicationNotes = XXHuge;

    // ========================================
    // Branch
    // ========================================
    
    /// <summary>Branch name</summary>
    public const int BranchName = XXLarge;
    
    /// <summary>Branch code</summary>
    public const int BranchCode = Medium;
    
    /// <summary>Branch address</summary>
    public const int BranchAddress = Huge;
    
    /// <summary>Branch phone</summary>
    public const int BranchPhone = Medium;

    // ========================================
    // Staff
    // ========================================
    
    /// <summary>Staff name</summary>
    public const int StaffName = XXLarge;
    
    /// <summary>Staff employee number</summary>
    public const int StaffEmployeeNumber = Large;
    
    /// <summary>Staff position</summary>
    public const int StaffPosition = XLarge;
    
    /// <summary>Staff email</summary>
    public const int StaffEmail = XXLarge;
    
    /// <summary>Staff phone</summary>
    public const int StaffPhone = Medium;

    // ========================================
    // Loan Product
    // ========================================
    
    /// <summary>Loan product name</summary>
    public const int LoanProductName = XXLarge;
    
    /// <summary>Loan product code</summary>
    public const int LoanProductCode = Medium;
    
    /// <summary>Loan product description</summary>
    public const int LoanProductDescription = XHuge;

    // ========================================
    // Savings Product
    // ========================================
    
    /// <summary>Savings product name</summary>
    public const int SavingsProductName = XXLarge;
    
    /// <summary>Savings product code</summary>
    public const int SavingsProductCode = Medium;
    
    /// <summary>Savings product description</summary>
    public const int SavingsProductDescription = XHuge;

    // ========================================
    // Common Fields
    // ========================================
    
    /// <summary>Generic name field</summary>
    public const int Name = XXLarge;
    
    /// <summary>Generic description field</summary>
    public const int Description = Huge;
    
    /// <summary>Generic notes field</summary>
    public const int Notes = XXHuge;
    
    /// <summary>Generic code field</summary>
    public const int Code = Medium;
    
    /// <summary>Generic reference number field</summary>
    public const int ReferenceNumber = Large;
    
    /// <summary>Generic status field</summary>
    public const int Status = Medium;
}
