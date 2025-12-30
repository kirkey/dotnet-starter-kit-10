namespace FSH.Framework.Core.Domain;
public interface IAuditableEntity
{
    DateTimeOffset CreatedOnUtc { get; }
    Guid? CreatedBy { get; }
    string? CreatedByUserName { get; }
    DateTimeOffset? LastModifiedOnUtc { get; }
    Guid? LastModifiedBy { get; }
    string? LastModifiedByUserName { get; }
}