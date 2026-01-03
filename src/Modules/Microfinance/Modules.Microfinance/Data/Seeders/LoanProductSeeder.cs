using FSH.Modules.Microfinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Modules.Microfinance.Data.Seeders;

/// <summary>
/// Seeder for loan products.
/// Prioritizes Salary Loan products as the primary offering, with both Straight (Flat) 
/// and Diminishing (Declining) payment options. Also seeds other microfinance products
/// like agricultural, micro-business, emergency, and educational loans.
/// </summary>
internal static class LoanProductSeeder
{
    public static async Task SeedAsync(
        MicrofinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 15;
        var existingCount = await context.LoanProducts.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        // Products are ordered by priority - Salary Loans FIRST as the primary product
        var products = new (string Code, string Name, string Desc, decimal MinAmt, decimal MaxAmt, decimal Rate, string Method, int MinTerm, int MaxTerm, string Freq, int GraceDays, decimal PenaltyRate)[]
        {
            // ========== SALARY LOANS (Priority Products) ==========
            // These are the primary products for the SaaS client focusing on salary-based lending
            
            ("SAL-STRAIGHT-01", "Salary Loan - Straight", 
                "Fixed monthly payments with equal principal and interest throughout the term. Best for employees who prefer predictable monthly deductions. Interest calculated on original principal (Flat rate method).", 
                5000, 500000, 12, "Flat", 3, 24, "Monthly", 0, 2),
            
            ("SAL-DIMINISH-01", "Salary Loan - Diminishing", 
                "Reducing monthly payments as principal decreases. More cost-effective for borrowers as interest is calculated on remaining balance. Ideal for employees who can handle higher initial payments.", 
                5000, 500000, 12, "Declining", 3, 24, "Monthly", 0, 2),
            
            ("SAL-EXPRESS-01", "Salary Loan Express", 
                "Quick disbursement salary loan with faster processing. For urgent financial needs with payroll deduction. Available for employees with 6+ months employment history.", 
                1000, 100000, 15, "Flat", 1, 12, "Monthly", 0, 3),
            
            ("SAL-PREMIUM-01", "Salary Loan Premium", 
                "Higher loan limits for senior employees and managers. Extended terms with competitive rates. Requires 2+ years employment and supervisor recommendation.", 
                50000, 1000000, 10, "Declining", 6, 36, "Monthly", 0, 1.5m),
            
            ("SAL-CONSOL-01", "Salary Loan Consolidation", 
                "Consolidate existing debts into a single manageable loan with payroll deduction. Lower interest rates for debt restructuring. Includes financial counseling.", 
                10000, 750000, 11, "Declining", 6, 48, "Monthly", 0, 2),

            // ========== AGRICULTURAL LOANS ==========
            ("AGRI-SEASONAL", "Agricultural Seasonal Loan", 
                "Financing for planting and harvesting seasons. Flexible repayment aligned with crop cycles. Includes crop insurance coverage option.", 
                10000, 200000, 10, "Flat", 3, 12, "Monthly", 30, 2),
            
            ("AGRI-EQUIPMENT", "Agricultural Equipment Loan", 
                "Purchase farming equipment and machinery. Extended terms for capital investments. Technical assessment support included.", 
                25000, 500000, 9, "Declining", 12, 60, "Monthly", 60, 1.5m),

            // ========== MICRO BUSINESS LOANS ==========
            ("MICRO-WORKING", "Micro Business Working Capital", 
                "Short-term working capital for small businesses. Quick approval process for existing entrepreneurs. Weekly or monthly repayment options.", 
                5000, 100000, 18, "Flat", 3, 12, "Monthly", 0, 3),
            
            ("MICRO-STARTUP", "Micro Enterprise Startup Loan", 
                "Seed funding for new micro businesses. Includes business mentorship program. Grace period for business establishment.", 
                10000, 150000, 15, "Declining", 6, 24, "Monthly", 30, 2.5m),

            // ========== EMERGENCY & PERSONAL LOANS ==========
            ("EMERG-QUICK", "Emergency Quick Loan", 
                "Rapid disbursement for emergencies. Minimal documentation requirements. Same-day processing for qualified members.", 
                1000, 50000, 20, "Flat", 1, 6, "Weekly", 0, 4),
            
            ("PERSONAL-GEN", "Personal General Purpose Loan", 
                "Flexible financing for personal needs. No collateral required for small amounts. Multiple repayment frequency options.", 
                2000, 100000, 16, "Declining", 3, 18, "Monthly", 0, 2.5m),

            // ========== EDUCATIONAL LOANS ==========
            ("EDU-SCHOOL", "Education School Fees Loan", 
                "Cover tuition, books, and school supplies. Semester-aligned disbursements available. Extended grace period during study.", 
                5000, 200000, 8, "Declining", 6, 48, "Monthly", 90, 1.5m),

            // ========== HOUSING LOANS ==========
            ("HOUSING-IMPROVE", "Housing Improvement Loan", 
                "Home repairs, renovations, and improvements. Property assessment included. Flexible collateral requirements.", 
                20000, 500000, 11, "Declining", 12, 60, "Monthly", 0, 2),

            // ========== GROUP LOANS ==========
            ("GROUP-SOLIDARITY", "Group Solidarity Loan", 
                "Joint liability group lending. Weekly meetings and repayments. Peer support and accountability.", 
                1000, 25000, 14, "Flat", 3, 12, "Weekly", 0, 2),

            // ========== ASSET FINANCE ==========
            ("ASSET-VEHICLE", "Asset Finance - Vehicle", 
                "Purchase motorcycles, tricycles, or commercial vehicles. Asset serves as collateral. Comprehensive insurance required.", 
                30000, 750000, 12, "Declining", 12, 48, "Monthly", 0, 2),
        };

        for (int i = existingCount; i < products.Length; i++)
        {
            var p = products[i];
            if (await context.LoanProducts.AnyAsync(x => x.Code == p.Code, cancellationToken).ConfigureAwait(false))
                continue;

            var product = LoanProduct.Create(
                code: p.Code,
                name: p.Name,
                description: p.Desc,
                minLoanAmount: p.MinAmt,
                maxLoanAmount: p.MaxAmt,
                interestRate: p.Rate,
                interestMethod: p.Method,
                minTermMonths: p.MinTerm,
                maxTermMonths: p.MaxTerm,
                repaymentFrequency: p.Freq,
                gracePeriodDays: p.GraceDays,
                latePenaltyRate: p.PenaltyRate);

            await context.LoanProducts.AddAsync(product, cancellationToken).ConfigureAwait(false);
        }

        logger.LogInformation("[{Tenant}] seeded loan products (Salary Loans prioritized)", tenant);
    }
}
