using FSH.Modules.Microfinance.Domain;
using Microsoft.Extensions.Logging;

namespace FSH.Modules.Microfinance.Data.Seeders;

/// <summary>
/// Seeder for loans with comprehensive test data.
/// Creates 150 loans across all statuses for realistic demo database.
/// </summary>
internal static class LoanSeeder
{
    public static async Task SeedAsync(
        MicrofinanceDbContext context,
        ILogger logger,
        string tenant,
        CancellationToken cancellationToken)
    {
        const int targetCount = 150;
        var existingCount = await context.Loans.CountAsync(cancellationToken).ConfigureAwait(false);
        if (existingCount >= targetCount) return;

        var members = await context.Members.Where(m => m.IsActive).Take(150).ToListAsync(cancellationToken).ConfigureAwait(false);
        var products = await context.LoanProducts.Where(p => p.IsActive).ToListAsync(cancellationToken).ConfigureAwait(false);

        if (members.Count < 10 || products.Count < 3) return;

        // Get product references - Salary Loans first (priority products)
        var salaryLoanStraight = products.FirstOrDefault(p => p.Code == "SAL-STRAIGHT-01") ?? products[0];
        var salaryLoanDiminish = products.FirstOrDefault(p => p.Code == "SAL-DIMINISH-01") ?? products[0];
        var salaryLoanExpress = products.FirstOrDefault(p => p.Code == "SAL-EXPRESS-01") ?? products[0];
        var salaryLoanPremium = products.FirstOrDefault(p => p.Code == "SAL-PREMIUM-01") ?? products[0];
        var salaryLoanConsol = products.FirstOrDefault(p => p.Code == "SAL-CONSOL-01") ?? products[0];
        
        // Other product references
        var personalLoan = products.FirstOrDefault(p => p.Code == "PERSONAL-LOAN") ?? products[0];
        var agriLoan = products.FirstOrDefault(p => p.Code == "AGRI-LOAN") ?? products[0];
        var microBusiness = products.FirstOrDefault(p => p.Code == "MICRO-BUSINESS") ?? products[0];
        var emergencyLoan = products.FirstOrDefault(p => p.Code == "EMERGENCY-LOAN") ?? products[0];
        var eduLoan = products.FirstOrDefault(p => p.Code == "EDU-LOAN") ?? products[0];

        var loanData = new (int MemberIdx, LoanProduct Product, decimal Amount, int Term, string Purpose, string Status)[]
        {
            // ========== SALARY LOANS - PRIORITY PRODUCTS (First 30 loans) ==========
            // Salary Loan Straight (Flat Payment) - Various Statuses
            (0, salaryLoanStraight, 25000, 12, "Home renovation - predictable monthly deductions", "Pending"),
            (1, salaryLoanStraight, 50000, 24, "Vehicle purchase - Toyota Vios downpayment", "Pending"),
            (2, salaryLoanStraight, 30000, 18, "Wedding expenses - church and reception", "Approved"),
            (3, salaryLoanStraight, 40000, 12, "Emergency medical - hospitalization of parent", "Approved"),
            (4, salaryLoanStraight, 75000, 24, "Educational expenses - MBA program", "Disbursed"),
            (5, salaryLoanStraight, 35000, 18, "Debt consolidation - credit card payoff", "Disbursed"),
            (6, salaryLoanStraight, 45000, 12, "Home appliances - complete house furnishing", "Disbursed"),
            (7, salaryLoanStraight, 60000, 24, "Computer setup - work from home equipment", "Disbursed"),
            (8, salaryLoanStraight, 20000, 6, "Travel expenses - family vacation", "Disbursed"),
            
            // Salary Loan Diminishing (Declining Payment) - Various Statuses
            (9, salaryLoanDiminish, 80000, 24, "House construction - additional room", "Pending"),
            (10, salaryLoanDiminish, 100000, 36, "Lot purchase - residential property", "Pending"),
            (11, salaryLoanDiminish, 60000, 24, "Vehicle purchase - motorcycle for commute", "Approved"),
            (12, salaryLoanDiminish, 90000, 36, "Business capital - spouse's sari-sari store", "Approved"),
            (13, salaryLoanDiminish, 150000, 48, "House renovation - major repairs", "Disbursed"),
            (14, salaryLoanDiminish, 120000, 36, "Educational expenses - child's college tuition", "Disbursed"),
            (15, salaryLoanDiminish, 75000, 24, "Medical expenses - dental surgery", "Disbursed"),
            (16, salaryLoanDiminish, 50000, 18, "Appliance purchase - refrigerator and AC", "Disbursed"),
            (17, salaryLoanDiminish, 85000, 24, "Home improvement - kitchen renovation", "Disbursed"),
            
            // Salary Loan Express (Quick Disbursement)
            (18, salaryLoanExpress, 15000, 6, "Emergency medical - child's hospitalization", "Disbursed"),
            (19, salaryLoanExpress, 20000, 9, "Emergency car repair - engine overhaul", "Disbursed"),
            (20, salaryLoanExpress, 10000, 6, "Urgent travel - family emergency abroad", "Approved"),
            (21, salaryLoanExpress, 25000, 12, "Tuition fee - semester payment deadline", "Pending"),
            
            // Salary Loan Premium (Higher Limits)
            (22, salaryLoanPremium, 200000, 48, "House and lot downpayment - subdivision", "Approved"),
            (23, salaryLoanPremium, 250000, 60, "Business capital - restaurant franchise", "Disbursed"),
            (24, salaryLoanPremium, 180000, 48, "Vehicle purchase - brand new SUV", "Disbursed"),
            (25, salaryLoanPremium, 300000, 60, "Property investment - condo unit", "Pending"),
            
            // Salary Loan Consolidation (Debt Restructuring)
            (26, salaryLoanConsol, 100000, 36, "Multiple credit card debts consolidation", "Disbursed"),
            (27, salaryLoanConsol, 150000, 48, "Previous loan restructuring - better terms", "Disbursed"),
            (28, salaryLoanConsol, 80000, 24, "Personal loan consolidation from 3 banks", "Approved"),
            (29, salaryLoanConsol, 120000, 36, "Debt consolidation - utility arrears", "Pending"),
            
            // ========== OTHER LOAN PRODUCTS (Loans 30-149) ==========
            // Pending loans - waiting for approval
            (30, agriLoan, 5000, 12, "Pagbili ng pataba at binhi para sa pagtatanim ng mais", "Pending"),
            (31, agriLoan, 8000, 12, "Irrigation equipment para sa gulay", "Pending"),
            (32, personalLoan, 2000, 6, "Pag-aayos ng bahay", "Pending"),
            (33, microBusiness, 10000, 24, "Pagpapalawak ng grocery store inventory", "Pending"),
            (34, emergencyLoan, 500, 3, "Medical emergency expenses", "Pending"),
            (35, personalLoan, 3500, 12, "Pagbayad ng tuition ng anak", "Pending"),
            (36, agriLoan, 7000, 12, "Pagbili ng kalabaw para sa araro", "Pending"),
            (37, microBusiness, 15000, 18, "Catering business expansion", "Pending"),
            
            // Approved loans - ready for disbursement
            (38, personalLoan, 3000, 12, "Vehicle repair at maintenance", "Approved"),
            (39, microBusiness, 15000, 36, "Pagbukas ng pangalawang tindahan", "Approved"),
            (40, eduLoan, 5000, 24, "University tuition fees", "Approved"),
            (41, agriLoan, 12000, 12, "Tractor rental at land preparation", "Approved"),
            (42, personalLoan, 1500, 6, "Gastos sa kasal ng anak", "Approved"),
            (43, microBusiness, 8000, 12, "Restaurant supplies", "Approved"),
            (44, eduLoan, 4000, 18, "Technical-Vocational training", "Approved"),
            (45, emergencyLoan, 1000, 3, "Emergency hospitalization", "Approved"),
            
            // Active/Disbursed loans - in repayment
            (46, microBusiness, 8000, 18, "Pagbili ng kagamitan sa panaderia", "Disbursed"),
            (47, agriLoan, 6000, 12, "Poultry farm expansion", "Disbursed"),
            (48, personalLoan, 4000, 12, "Motorcycle purchase para sa negosyo", "Disbursed"),
            (49, eduLoan, 8000, 36, "Professional certification program", "Disbursed"),
            (50, microBusiness, 20000, 24, "Restaurant renovation", "Disbursed"),
            (51, personalLoan, 2500, 12, "Solar panel installation", "Disbursed"),
            (52, agriLoan, 10000, 12, "Dairy cattle purchase", "Disbursed"),
            (53, microBusiness, 5000, 12, "Tailoring shop equipment", "Disbursed"),
            (54, emergencyLoan, 1000, 3, "Roof repair after storm damage", "Disbursed"),
            (55, personalLoan, 3500, 18, "Computer at office equipment", "Disbursed"),
            (56, agriLoan, 9000, 12, "Rice farming inputs", "Disbursed"),
            (57, microBusiness, 12000, 24, "Bakery equipment upgrade", "Disbursed"),
            (58, personalLoan, 2000, 9, "Home appliances purchase", "Disbursed"),
            (59, eduLoan, 6000, 24, "Masters degree tuition", "Disbursed"),
            (60, microBusiness, 18000, 36, "Grocery store franchise", "Disbursed"),
            (61, agriLoan, 7500, 12, "Fishpond inputs at feeds", "Disbursed"),
            (62, personalLoan, 4500, 18, "Wedding expenses", "Disbursed"),
            (63, microBusiness, 25000, 36, "Beauty salon renovation", "Disbursed"),
            (64, emergencyLoan, 1500, 6, "Medical operation expenses", "Disbursed"),
            (65, eduLoan, 10000, 36, "Nursing school complete tuition", "Disbursed"),
            
            // Rejected loans - for testing rejection workflow
            (66, microBusiness, 50000, 48, "Large factory expansion - exceeds limit", "Rejected"),
            (67, personalLoan, 15000, 6, "Insufficient income documentation", "Rejected"),
            (68, agriLoan, 30000, 24, "Land purchase - outside policy scope", "Rejected"),
            (69, eduLoan, 20000, 12, "No valid enrollment documents", "Rejected"),
            (70, microBusiness, 40000, 36, "Business plan not viable", "Rejected"),
            
            // More pending for variety
            (71, eduLoan, 3000, 12, "Technical training course fees", "Pending"),
            (72, microBusiness, 7500, 18, "Food truck business startup", "Pending"),
            (73, personalLoan, 1800, 9, "Furniture purchase for new home", "Pending"),
            (74, agriLoan, 4500, 6, "Seasonal crop inputs - beans", "Pending"),
            (75, emergencyLoan, 800, 2, "Vehicle breakdown repair", "Pending"),
            (76, microBusiness, 12000, 24, "Beauty salon equipment", "Pending"),
            (77, eduLoan, 6000, 36, "Nursing school tuition", "Pending"),
            (78, personalLoan, 2500, 12, "Home renovation - kitchen", "Pending"),
            (79, agriLoan, 5500, 12, "Vegetable greenhouse construction", "Pending"),
            
            // More disbursed for comprehensive testing
            (80, microBusiness, 11000, 18, "Carinderia equipment", "Disbursed"),
            (81, agriLoan, 8500, 12, "Pig farming inputs", "Disbursed"),
            (82, personalLoan, 3000, 12, "Computer for WFH setup", "Disbursed"),
            (83, eduLoan, 5500, 24, "IT certification course", "Disbursed"),
            (84, microBusiness, 16000, 24, "Auto repair shop tools", "Disbursed"),
            (85, emergencyLoan, 2000, 6, "House fire damage repair", "Disbursed"),
            (86, agriLoan, 11000, 12, "Duck farming expansion", "Disbursed"),
            (87, personalLoan, 4000, 18, "Motorcycle sidecar for business", "Disbursed"),
            (88, microBusiness, 22000, 36, "Laundry shop equipment", "Disbursed"),
            (89, eduLoan, 7000, 24, "Culinary arts program", "Disbursed"),
            
            // Additional loans for comprehensive demo (Loan 90-149)
            // More Pending Loans
            (90, agriLoan, 6500, 12, "Cassava farming inputs", "Pending"),
            (91, microBusiness, 9000, 18, "Cellphone repair shop equipment", "Pending"),
            (92, personalLoan, 2800, 12, "Home appliance purchase", "Pending"),
            (93, eduLoan, 4500, 24, "Accounting certification course", "Pending"),
            (94, emergencyLoan, 700, 3, "Emergency dental procedure", "Pending"),
            (95, agriLoan, 8500, 12, "Goat farming startup", "Pending"),
            (96, microBusiness, 11000, 24, "Internet cafe equipment", "Pending"),
            (97, personalLoan, 3200, 18, "Wedding expenses", "Pending"),
            (98, agriLoan, 5500, 6, "Seasonal vegetable inputs", "Pending"),
            (99, microBusiness, 14000, 24, "Water refilling station equipment", "Pending"),
            (100, eduLoan, 5000, 18, "Welding certification", "Pending"),
            (101, personalLoan, 1500, 6, "Medical checkup expenses", "Pending"),
            (102, emergencyLoan, 900, 3, "Flood damage repairs", "Pending"),
            (103, agriLoan, 7500, 12, "Tilapia pond inputs", "Pending"),
            (104, microBusiness, 8000, 18, "Printing shop equipment", "Pending"),
            
            // More Approved Loans
            (105, agriLoan, 9500, 12, "Coffee plantation inputs", "Approved"),
            (106, microBusiness, 16000, 24, "Hardware store inventory", "Approved"),
            (107, personalLoan, 3800, 18, "Motorcycle purchase", "Approved"),
            (108, eduLoan, 6500, 24, "IT bootcamp tuition", "Approved"),
            (109, emergencyLoan, 1200, 6, "Hospitalization expenses", "Approved"),
            (110, agriLoan, 10500, 12, "Cacao farming inputs", "Approved"),
            (111, microBusiness, 13000, 24, "Bicycle rental business", "Approved"),
            (112, personalLoan, 2500, 12, "Computer purchase for WFH", "Approved"),
            (113, eduLoan, 3500, 18, "Driving school tuition", "Approved"),
            (114, agriLoan, 6000, 12, "Organic vegetable farming", "Approved"),
            (115, microBusiness, 18000, 36, "Motorcycle spare parts shop", "Approved"),
            (116, personalLoan, 4200, 24, "Home improvement loan", "Approved"),
            (117, emergencyLoan, 800, 3, "Emergency surgery down payment", "Approved"),
            (118, eduLoan, 7500, 36, "Engineering review course", "Approved"),
            (119, agriLoan, 8000, 12, "Mushroom farming inputs", "Approved"),
            
            // More Disbursed/Active Loans
            (120, microBusiness, 19000, 36, "Coffee shop setup", "Disbursed"),
            (121, agriLoan, 12500, 12, "Rice milling equipment", "Disbursed"),
            (122, personalLoan, 5500, 24, "Solar panel installation", "Disbursed"),
            (123, eduLoan, 8500, 36, "Medical technician course", "Disbursed"),
            (124, microBusiness, 21000, 36, "Auto parts store", "Disbursed"),
            (125, emergencyLoan, 1800, 6, "Typhoon damage repairs", "Disbursed"),
            (126, agriLoan, 9000, 12, "Vegetable greenhouse", "Disbursed"),
            (127, microBusiness, 14500, 24, "Carwash business expansion", "Disbursed"),
            (128, personalLoan, 3500, 18, "Educational laptop purchase", "Disbursed"),
            (129, eduLoan, 6000, 24, "Bartending course", "Disbursed"),
            (130, agriLoan, 11500, 12, "Dairy cattle purchase", "Disbursed"),
            (131, microBusiness, 23000, 36, "Pharmacy inventory", "Disbursed"),
            (132, personalLoan, 4800, 24, "Renovation for home business", "Disbursed"),
            (133, emergencyLoan, 2200, 6, "Vehicle accident repairs", "Disbursed"),
            (134, eduLoan, 9000, 36, "Pilot training course", "Disbursed"),
            (135, agriLoan, 7000, 12, "Fruit orchard inputs", "Disbursed"),
            (136, microBusiness, 17000, 24, "Clothing boutique inventory", "Disbursed"),
            (137, personalLoan, 2200, 12, "Emergency travel expenses", "Disbursed"),
            (138, agriLoan, 8500, 12, "Corn processing equipment", "Disbursed"),
            (139, microBusiness, 20000, 36, "Food processing business", "Disbursed"),
            (140, eduLoan, 5500, 24, "Culinary training", "Disbursed"),
            (141, agriLoan, 6500, 12, "Aquaculture pond inputs", "Disbursed"),
            (142, microBusiness, 12000, 24, "Salon equipment upgrade", "Disbursed"),
            (143, personalLoan, 3000, 18, "Home security installation", "Disbursed"),
            (144, emergencyLoan, 1500, 6, "House fire emergency", "Disbursed"),
            (145, agriLoan, 10000, 12, "Hydroponic farming setup", "Disbursed"),
            (146, microBusiness, 25000, 36, "Cooperative store setup", "Disbursed"),
            (147, eduLoan, 7000, 24, "Maritime training", "Disbursed"),
            (148, personalLoan, 4500, 24, "Working capital for freelancing", "Disbursed"),
            (149, agriLoan, 5000, 6, "Seasonal crop inputs", "Disbursed"),
        };

        int loanNumber = 3001;
        for (int i = 0; i < loanData.Length && i + existingCount < targetCount; i++)
        {
            var data = loanData[i];
            var loanNum = $"LN-{loanNumber + i:D6}";
            
            if (await context.Loans.AnyAsync(l => l.LoanNumber == loanNum, cancellationToken).ConfigureAwait(false))
                continue;

            var member = members[data.MemberIdx % members.Count];
            var product = data.Product;

            var loan = Loan.Create(
                memberId: member.Id,
                loanProductId: product.Id,
                loanNumber: loanNum,
                principalAmount: data.Amount,
                interestRate: product.InterestRate,
                termMonths: data.Term,
                repaymentFrequency: product.RepaymentFrequency,
                purpose: data.Purpose);

            // Apply status transitions based on target status
            switch (data.Status)
            {
                case "Approved":
                    loan.Approve(DateTime.UtcNow.AddDays(-Random.Shared.Next(5, 30)));
                    break;
                    
                case "Disbursed":
                    loan.Approve(DateTime.UtcNow.AddDays(-Random.Shared.Next(60, 120)));
                    loan.Disburse(
                        DateTime.UtcNow.AddDays(-Random.Shared.Next(30, 90)),
                        DateTime.UtcNow.AddMonths(data.Term));
                    break;
                    
                case "Rejected":
                    loan.Reject("Does not meet lending criteria - " + data.Purpose.Split('-').Last().Trim());
                    break;
            }

            await context.Loans.AddAsync(loan, cancellationToken).ConfigureAwait(false);
        }

        await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("[{Tenant}] seeded {Count} loans across all statuses (Pending, Approved, Disbursed, Rejected)", tenant, targetCount);
    }
}
