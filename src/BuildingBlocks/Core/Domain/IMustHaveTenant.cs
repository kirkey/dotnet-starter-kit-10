namespace FSH.Framework.Core.Domain;

/// <summary>
/// Marker interface for entities that must have a tenant id.
/// </summary>
public interface IMustHaveTenant
{
    string TenantId { get; set; }
}