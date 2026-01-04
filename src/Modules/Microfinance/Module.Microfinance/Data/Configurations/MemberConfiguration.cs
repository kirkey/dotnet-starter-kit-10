using FSH.Module.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Module.Microfinance.Data.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.ToTable("Members", "microfinance");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.MemberNumber)
            .IsRequired()
            .HasMaxLength(Member.MemberNumberMaxLength);

        builder.HasIndex(m => m.MemberNumber)
            .IsUnique();

        builder.Property(m => m.FirstName)
            .IsRequired()
            .HasMaxLength(Member.FirstNameMaxLength);

        builder.Property(m => m.LastName)
            .IsRequired()
            .HasMaxLength(Member.LastNameMaxLength);

        builder.Property(m => m.MiddleName)
            .HasMaxLength(Member.MiddleNameMaxLength);

        builder.Property(m => m.Email)
            .HasMaxLength(Member.EmailMaxLength);

        builder.Property(m => m.PhoneNumber)
            .HasMaxLength(Member.PhoneNumberMaxLength);

        builder.Property(m => m.Gender)
            .HasMaxLength(Member.GenderMaxLength);

        builder.Property(m => m.Address)
            .HasMaxLength(Member.AddressMaxLength);

        builder.Property(m => m.NationalId)
            .HasMaxLength(Member.NationalIdMaxLength);

        builder.Property(m => m.Occupation)
            .HasMaxLength(Member.OccupationMaxLength);

        builder.Property(m => m.MonthlyIncome)
            .HasColumnType("decimal(18,2)");

        builder.Property(m => m.JoinDate)
            .IsRequired();

        builder.Property(m => m.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        // Indexes for common queries
        builder.HasIndex(m => m.IsActive);
        builder.HasIndex(m => m.PhoneNumber);
        builder.HasIndex(m => m.Email);
        builder.HasIndex(m => m.NationalId);
        builder.HasIndex(m => m.TenantId);
    }
}
