using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Domain;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Ai.Services;

/// <summary>
/// Reconciles the runtime catalog against a detection pass: known families always have rows,
/// seen entries go online with versions, and previously-seen entries that disappear flip to
/// offline — while agents and history bound to them are untouched.
/// </summary>
public sealed class RuntimeCatalogService(AiDbContext db)
{
    public async Task<IReadOnlyList<AiRuntime>> ReconcileAsync(
        IReadOnlyList<DetectedAgent> detected, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(detected);

        var rows = await db.Runtimes.ToListAsync(ct).ConfigureAwait(false);
        var desired = RuntimeReconciliation.Compute(
            RuntimeVariantMatrix.KnownFamilyNames(),
            rows.ToDictionary(
                r => r.Family,
                r => (r.DetectedVersion, r.IsOnline),
                StringComparer.OrdinalIgnoreCase),
            detected);

        foreach (var want in desired)
        {
            var row = rows.FirstOrDefault(r =>
                string.Equals(r.Family, want.Family, StringComparison.OrdinalIgnoreCase));
            if (row is null)
            {
                row = AiRuntime.Register(
                    want.Family, want.DisplayName, want.IsOnline ? "detection" : "manual");
                db.Runtimes.Add(row);
                rows.Add(row);
            }

            if (want.IsOnline)
            {
                row.ReportSeen(want.DetectedVersion);
            }
            else
            {
                row.ReportMissing();
            }
        }

        await db.SaveChangesAsync(ct).ConfigureAwait(false);
        return rows.OrderBy(r => r.Family, StringComparer.Ordinal).ToList();
    }
}
