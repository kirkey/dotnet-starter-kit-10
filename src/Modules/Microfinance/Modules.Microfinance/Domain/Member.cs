namespace FSH.Modules.Microfinance.Domain;

/// <summary>
/// Represents a member (customer/client) in the microfinance system.
/// </summary>
public class Member : AuditableEntity<Guid>
{
    public const int MemberNumberMaxLength = 64;
    public const int FirstNameMaxLength = 128;
    public const int LastNameMaxLength = 128;
    public const int MiddleNameMaxLength = 128;
    public const int EmailMaxLength = 256;
    public const int PhoneNumberMaxLength = 32;
    public const int AddressMaxLength = 512;
    public const int NationalIdMaxLength = 64;
    public const int OccupationMaxLength = 256;
    public const int GenderMaxLength = 32;
    public const int FirstNameMinLength = 2;
    public const int LastNameMinLength = 2;

    public string MemberNumber { get; private set; } = default!;
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string? MiddleName { get; private set; }
    public string FullName => string.IsNullOrWhiteSpace(MiddleName)
        ? $"{FirstName} {LastName}"
        : $"{FirstName} {MiddleName} {LastName}";
    public string? Email { get; private set; }
    public string? PhoneNumber { get; private set; }
    public DateTimeOffset? DateOfBirth { get; private set; }
    public string? Gender { get; private set; }
    public string? Address { get; private set; }
    public string? NationalId { get; private set; }
    public string? Occupation { get; private set; }
    public decimal? MonthlyIncome { get; private set; }
    public DateTimeOffset JoinDate { get; private set; }
    public bool IsActive { get; private set; }

    private Member() { }

    public static Member Create(
        string memberNumber,
        string firstName,
        string lastName,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? middleName = null,
        string? email = null,
        string? phoneNumber = null,
        DateTimeOffset? dateOfBirth = null,
        string? gender = null,
        string? address = null,
        string? nationalId = null,
        string? occupation = null,
        decimal? monthlyIncome = null,
        DateTimeOffset? joinDate = null)
    {
        var member = new Member
        {
            Id = Guid.NewGuid(),
            MemberNumber = memberNumber.Trim(),
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            MiddleName = middleName?.Trim(),
            Email = email?.Trim(),
            PhoneNumber = phoneNumber?.Trim(),
            DateOfBirth = dateOfBirth,
            Gender = gender?.Trim(),
            Address = address?.Trim(),
            NationalId = nationalId?.Trim(),
            Occupation = occupation?.Trim(),
            MonthlyIncome = monthlyIncome,
            JoinDate = joinDate ?? DateTimeOffset.UtcNow,
            IsActive = true,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName
        };

        return member;
    }

    public void Update(
        string? firstName,
        string? lastName,
        string? middleName,
        string? email,
        string? phoneNumber,
        DateTimeOffset? dateOfBirth,
        string? gender,
        string? address,
        string? nationalId,
        string? occupation,
        decimal? monthlyIncome)
    {
        if (!string.IsNullOrWhiteSpace(firstName)) FirstName = firstName.Trim();
        if (!string.IsNullOrWhiteSpace(lastName)) LastName = lastName.Trim();
        MiddleName = middleName?.Trim();
        Email = email?.Trim();
        PhoneNumber = phoneNumber?.Trim();
        DateOfBirth = dateOfBirth;
        Gender = gender?.Trim();
        Address = address?.Trim();
        NationalId = nationalId?.Trim();
        Occupation = occupation?.Trim();
        MonthlyIncome = monthlyIncome;
    }

    public void Activate()
    {
        if (!IsActive)
        {
            IsActive = true;
        }
    }

    public void Deactivate()
    {
        if (IsActive)
        {
            IsActive = false;
        }
    }
}
