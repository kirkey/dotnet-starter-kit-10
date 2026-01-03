using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Data.Seeders;

internal static class MicroFinanceScaleSeeder
{
    /// <summary>
    /// Adds additional sample data to support load / scalability testing.
    /// Controlled by the environment variable `SEED_SCALE` (int).
    /// If scale &gt; 1, we expand member counts (and related simple accounts) proportionally.
    /// </summary>
    public static async Task SeedAsync(
        MicrofinanceDbContext context,
        ILogger logger,
        string tenant,
        int scale,
        CancellationToken cancellationToken)
    {
        if (scale <= 1) return;

        // Base sizes are aligned with existing seeders
        const int baseMembers = 250;
        var targetMembers = baseMembers * scale;

        var existingMembers = await context.Members.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingMembers >= targetMembers)
        {
            logger.LogInformation("[{Tenant}] microfinance scale seeding skipped: already have {Existing} members >= target {Target}", tenant, existingMembers, targetMembers);
            return;
        }

        logger.LogInformation("[{Tenant}] microfinance scale seeding starting: expanding members {Existing} -> {Target}", tenant, existingMembers, targetMembers);

        // Find a savings product to create accounts against (if available)
        var savingsProduct = await context.SavingsProducts.AsQueryable().FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        var random = new Random(12345 + scale);
        var created = 0;

        for (int i = existingMembers + 1; i <= targetMembers; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var num = $"MBR-{i:D6}";
            if (await context.Members.AnyAsync(x => x.MemberNumber == num, cancellationToken).ConfigureAwait(false))
                continue;

            var first = $"AutoF{random.Next(1000, 9999)}";
            var last = $"AutoL{random.Next(1000, 9999)}";

            var member = Member.Create(
                memberNumber: num,
                firstName: first,
                lastName: last,
                middleName: null,
                email: $"{first.ToLower()}.{last.ToLower()}{i}@example.com",
                phoneNumber: $"+639{random.Next(100000000, 999999999)}",
                dateOfBirth: DateTime.UtcNow.AddYears(-20 - (i % 40)).AddDays(random.Next(0, 365)),
                gender: (i % 2 == 0) ? "Male" : "Female",
                address: $"{1000 + i} LoadTest Street",
                nationalId: $"LT-NAT-{i:D8}",
                occupation: "LoadTest",
                monthlyIncome: random.Next(8000, 120000),
                joinDate: DateTime.UtcNow.AddDays(-random.Next(0, 365)));

            await context.Members.AddAsync(member, cancellationToken).ConfigureAwait(false);
            created++;

            // Create a savings account for some members (40%) to add transactional load
            if (savingsProduct != null && random.NextDouble() < 0.4)
            {
                var accNum = $"SAV-{i:D8}";
                var opening = random.Next(100, 20000);
                var account = SavingsAccount.Create(
                    accountNumber: accNum,
                    memberId: member.Id,
                    savingsProductId: savingsProduct.Id,
                    openingBalance: opening,
                    openedDate: DateTime.UtcNow);

                await context.SavingsAccounts.AddAsync(account, cancellationToken).ConfigureAwait(false);

                // Do a few deposits to create transactions
                var depositCount = random.Next(1, 4);
                for (int d = 0; d < depositCount; d++)
                {
                    var amount = random.Next(100, Math.Max(500, opening));
                    account.Deposit(amount);
                }
            }

            if (created % 500 == 0)
            {
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                logger.LogInformation("[{Tenant}] microfinance scale seeding progress: created {Created} members so far", tenant, created);
            }
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] microfinance scale seeding completed: added {Added} members (target {Target})", tenant, created, targetMembers);
    }
}
