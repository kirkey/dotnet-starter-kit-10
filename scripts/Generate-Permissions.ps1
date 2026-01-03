# Generate comprehensive permissions for all 75 entities

$entities = @(
    "Member", "AgentBanking", "AmlAlert", "ApprovalRequest", "ApprovalWorkflow",
    "Branch", "BranchTarget", "CashVault", "CollateralInsurance",
    "CollateralRelease", "CollateralType", "CollateralValuation", "CollectionAction",
    "CollectionCase", "CollectionStrategy", "CommunicationLog", "CommunicationTemplate",
    "CreditBureauInquiry", "CreditBureauReport", "CreditScore", "CustomerCase",
    "CustomerSegment", "CustomerSurvey", "DebtSettlement", "Document",
    "FeeCharge", "FeeDefinition", "FeePayment", "FeeWaiver",
    "FixedDeposit", "GroupMembership", "InsuranceClaim", "InsurancePolicy",
    "InsuranceProduct", "InterestRateChange", "InvestmentAccount", "InvestmentProduct",
    "InvestmentTransaction", "KycDocument", "LegalAction", "Loan",
    "LoanApplication", "LoanCollateral", "LoanDisbursementTranche", "LoanGuarantor",
    "LoanOfficerAssignment", "LoanOfficerTarget", "LoanProduct", "LoanRepayment",
    "LoanRestructure", "LoanSchedule", "LoanWriteOff", "MarketingCampaign",
    "MemberGroup", "MfiConfiguration", "MobileTransaction", "MobileWallet",
    "PaymentGateway", "PromiseToPay", "QrPayment", "ReportDefinition",
    "ReportGeneration", "RiskAlert", "RiskCategory", "RiskIndicator",
    "SavingsAccount", "SavingsProduct", "SavingsTransaction", "ShareAccount",
    "ShareProduct", "ShareTransaction", "Staff", "StaffTraining",
    "TellerSession", "UssdSession"
)

$content = @"
using FSH.Framework.Shared.Identity;

namespace FSH.Modules.Microfinance;

public static class MicrofinancePermissionConstants
{
"@

# Generate permission classes for each entity
foreach ($entity in $entities) {
    $entityPlural = "${entity}s"
    $content += @"

    public static class $entityPlural
    {
        public const string View = "Permissions.Microfinance.$entityPlural.View";
        public const string Search = "Permissions.Microfinance.$entityPlural.Search";
        public const string Create = "Permissions.Microfinance.$entityPlural.Create";
        public const string Update = "Permissions.Microfinance.$entityPlural.Update";
        public const string Delete = "Permissions.Microfinance.$entityPlural.Delete";
    }
"@
}

$content += @"

    public static IReadOnlyList<FshPermission> GetPermissions() => new List<FshPermission>
    {
"@

# Generate permission registrations
foreach ($entity in $entities) {
    $entityPlural = "${entity}s"
    $content += @"

        // $entityPlural permissions
        new("View $entityPlural", ActionConstants.View, ResourceConstants.Microfinance.$entityPlural, IsBasic: true),
        new("Search $entityPlural", ActionConstants.Search, ResourceConstants.Microfinance.$entityPlural, IsBasic: true),
        new("Create $entityPlural", ActionConstants.Create, ResourceConstants.Microfinance.$entityPlural),
        new("Update $entityPlural", ActionConstants.Update, ResourceConstants.Microfinance.$entityPlural),
        new("Delete $entityPlural", ActionConstants.Delete, ResourceConstants.Microfinance.$entityPlural),
"@
}

$content += @"

    };
}

public static class ResourceConstants
{
    public static class Microfinance
    {
"@

foreach ($entity in $entities) {
    $entityPlural = "${entity}s"
    $content += @"

        public const string $entityPlural = "Microfinance.$entityPlural";
"@
}

$content += @"

    }
}
"@

$outputPath = "/Users/kirkeypsalms/Projects/dotnet-starter-kit-10/src/Modules/Microfinance/Modules.Microfinance"
Set-Content -Path "$outputPath/MicrofinancePermissionConstants.cs" -Value $content

Write-Host "✓ MicrofinancePermissionConstants.cs generated successfully!" -ForegroundColor Green
