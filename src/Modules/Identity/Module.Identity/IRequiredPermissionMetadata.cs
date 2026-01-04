namespace FSH.Module.Identity;

public interface IRequiredPermissionMetadata
{
    HashSet<string> RequiredPermissions { get; }
}
