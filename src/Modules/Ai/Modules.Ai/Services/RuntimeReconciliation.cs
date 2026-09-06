namespace FSH.Modules.Ai.Services;

/// <summary>Pure reconciliation logic (no database): computes the desired end state of every
/// runtime row from the known families, existing rows, and a detection pass.</summary>
public static class RuntimeReconciliation
{
    public sealed record DesiredRow(string Family, string DisplayName, string? DetectedVersion, bool IsOnline, bool IsNew);

    public static IReadOnlyList<DesiredRow> Compute(
        IReadOnlyList<string> knownFamilies,
        IReadOnlyDictionary<string, (string? DetectedVersion, bool IsOnline)> existing,
        IReadOnlyList<DetectedAgent> detected)
    {
        ArgumentNullException.ThrowIfNull(knownFamilies);
        ArgumentNullException.ThrowIfNull(existing);
        ArgumentNullException.ThrowIfNull(detected);

        var seen = detected
            .GroupBy(d => d.Family, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.First().DetectedVersion, StringComparer.OrdinalIgnoreCase);

        return knownFamilies
            .Concat(seen.Keys)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Select(family =>
            {
                var isNew = !existing.ContainsKey(family);
                return seen.TryGetValue(family, out var version)
                    ? new DesiredRow(family, RuntimeVariantMatrix.DisplayName(family), version, IsOnline: true, isNew)
                    : new DesiredRow(family, RuntimeVariantMatrix.DisplayName(family), null, IsOnline: false, isNew);
            })
            .OrderBy(r => r.Family, StringComparer.Ordinal)
            .ToList();
    }
}
