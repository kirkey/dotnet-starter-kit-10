using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Persistence;
using FSH.Framework.Shared.Multitenancy;
using Microsoft.Extensions.Logging;

namespace FSH.Module.Microfinance.Data;

internal sealed class MicrofinanceDbInitializer(
    ILogger<MicrofinanceDbInitializer> logger,
    MicrofinanceDbContext context,
    IMultiTenantContextAccessor<AppTenantInfo> multiTenantContextAccessor) : IDbInitializer
{
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        if ((await context.Database.GetPendingMigrationsAsync(cancellationToken).ConfigureAwait(false)).Any())
        {
            await context.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            logger.LogInformation("[{Tenant}] applied database migrations for microfinance module",
                multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Identifier);
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        var tenant = multiTenantContextAccessor.MultiTenantContext.TenantInfo?.Identifier ?? "root";

        if (await context.Members.AnyAsync(cancellationToken))
        {
            logger.LogInformation("[{Tenant}] microfinance data already seeded", tenant);
            return;
        }

        logger.LogInformation("[{Tenant}] seeding microfinance module with basic demo data", tenant);

        // Note: Seed data files are available in Data/Seeders/ (64 files migrated)
        // They are temporarily excluded from build as they require business methods
        // on domain entities. Enable them incrementally as entities are enhanced.
        //
        // Available seeders: MemberSeeder, BranchSeeder, StaffSeeder, LoanProductSeeder,
        // SavingsProductSeeder, and 59 others covering all microfinance operations.
        
        // TODO: Add basic inline seed data here or enable seeders after enhancing entities

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] completed microfinance module initialization", tenant);
    }
}
