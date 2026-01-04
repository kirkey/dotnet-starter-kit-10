using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Persistence;
using FSH.Framework.Shared.Multitenancy;
using FSH.Framework.Shared.Persistence;
using FSH.Module.Microfinance.Domain;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace FSH.Module.Microfinance.Data;

public class MicrofinanceDbContext : DbContext
{
    private readonly DatabaseOptions _settings;
    public AppTenantInfo? TenantInfo { get; private set; }
    private readonly IHostEnvironment _environment;

    public DbSet<Member> Members => Set<Member>();
    public DbSet<AgentBanking> AgentBankings => Set<AgentBanking>();
    public DbSet<AmlAlert> AmlAlerts => Set<AmlAlert>();
    public DbSet<ApprovalRequest> ApprovalRequests => Set<ApprovalRequest>();
    public DbSet<ApprovalWorkflow> ApprovalWorkflows => Set<ApprovalWorkflow>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<BranchTarget> BranchTargets => Set<BranchTarget>();
    public DbSet<CashVault> CashVaults => Set<CashVault>();
    public DbSet<CollateralInsurance> CollateralInsurances => Set<CollateralInsurance>();
    public DbSet<CollateralRelease> CollateralReleases => Set<CollateralRelease>();
    public DbSet<CollateralType> CollateralTypes => Set<CollateralType>();
    public DbSet<CollateralValuation> CollateralValuations => Set<CollateralValuation>();
    public DbSet<CollectionAction> CollectionActions => Set<CollectionAction>();
    public DbSet<CollectionCase> CollectionCases => Set<CollectionCase>();
    public DbSet<CollectionStrategy> CollectionStrategys => Set<CollectionStrategy>();
    public DbSet<CommunicationLog> CommunicationLogs => Set<CommunicationLog>();
    public DbSet<CommunicationTemplate> CommunicationTemplates => Set<CommunicationTemplate>();
    public DbSet<CreditBureauInquiry> CreditBureauInquirys => Set<CreditBureauInquiry>();
    public DbSet<CreditBureauReport> CreditBureauReports => Set<CreditBureauReport>();
    public DbSet<CreditScore> CreditScores => Set<CreditScore>();
    public DbSet<CustomerCase> CustomerCases => Set<CustomerCase>();
    public DbSet<CustomerSegment> CustomerSegments => Set<CustomerSegment>();
    public DbSet<CustomerSurvey> CustomerSurveys => Set<CustomerSurvey>();
    public DbSet<DebtSettlement> DebtSettlements => Set<DebtSettlement>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<FeeCharge> FeeCharges => Set<FeeCharge>();
    public DbSet<FeeDefinition> FeeDefinitions => Set<FeeDefinition>();
    public DbSet<FeePayment> FeePayments => Set<FeePayment>();
    public DbSet<FeeWaiver> FeeWaivers => Set<FeeWaiver>();
    public DbSet<FixedDeposit> FixedDeposits => Set<FixedDeposit>();
    public DbSet<GroupMembership> GroupMemberships => Set<GroupMembership>();
    public DbSet<InsuranceClaim> InsuranceClaims => Set<InsuranceClaim>();
    public DbSet<InsurancePolicy> InsurancePolicys => Set<InsurancePolicy>();
    public DbSet<InsuranceProduct> InsuranceProducts => Set<InsuranceProduct>();
    public DbSet<InterestRateChange> InterestRateChanges => Set<InterestRateChange>();
    public DbSet<InvestmentAccount> InvestmentAccounts => Set<InvestmentAccount>();
    public DbSet<InvestmentProduct> InvestmentProducts => Set<InvestmentProduct>();
    public DbSet<InvestmentTransaction> InvestmentTransactions => Set<InvestmentTransaction>();
    public DbSet<KycDocument> KycDocuments => Set<KycDocument>();
    public DbSet<LegalAction> LegalActions => Set<LegalAction>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<LoanApplication> LoanApplications => Set<LoanApplication>();
    public DbSet<LoanCollateral> LoanCollaterals => Set<LoanCollateral>();
    public DbSet<LoanDisbursementTranche> LoanDisbursementTranches => Set<LoanDisbursementTranche>();
    public DbSet<LoanGuarantor> LoanGuarantors => Set<LoanGuarantor>();
    public DbSet<LoanOfficerAssignment> LoanOfficerAssignments => Set<LoanOfficerAssignment>();
    public DbSet<LoanOfficerTarget> LoanOfficerTargets => Set<LoanOfficerTarget>();
    public DbSet<LoanProduct> LoanProducts => Set<LoanProduct>();
    public DbSet<LoanRepayment> LoanRepayments => Set<LoanRepayment>();
    public DbSet<LoanRestructure> LoanRestructures => Set<LoanRestructure>();
    public DbSet<LoanSchedule> LoanSchedules => Set<LoanSchedule>();
    public DbSet<LoanWriteOff> LoanWriteOffs => Set<LoanWriteOff>();
    public DbSet<MarketingCampaign> MarketingCampaigns => Set<MarketingCampaign>();
    public DbSet<MemberGroup> MemberGroups => Set<MemberGroup>();
    public DbSet<MfiConfiguration> MfiConfigurations => Set<MfiConfiguration>();
    public DbSet<MobileTransaction> MobileTransactions => Set<MobileTransaction>();
    public DbSet<MobileWallet> MobileWallets => Set<MobileWallet>();
    public DbSet<PaymentGateway> PaymentGateways => Set<PaymentGateway>();
    public DbSet<PromiseToPay> PromiseToPays => Set<PromiseToPay>();
    public DbSet<QrPayment> QrPayments => Set<QrPayment>();
    public DbSet<ReportDefinition> ReportDefinitions => Set<ReportDefinition>();
    public DbSet<ReportGeneration> ReportGenerations => Set<ReportGeneration>();
    public DbSet<RiskAlert> RiskAlerts => Set<RiskAlert>();
    public DbSet<RiskCategory> RiskCategorys => Set<RiskCategory>();
    public DbSet<RiskIndicator> RiskIndicators => Set<RiskIndicator>();
    public DbSet<SavingsAccount> SavingsAccounts => Set<SavingsAccount>();
    public DbSet<SavingsProduct> SavingsProducts => Set<SavingsProduct>();
    public DbSet<SavingsTransaction> SavingsTransactions => Set<SavingsTransaction>();
    public DbSet<ShareAccount> ShareAccounts => Set<ShareAccount>();
    public DbSet<ShareProduct> ShareProducts => Set<ShareProduct>();
    public DbSet<ShareTransaction> ShareTransactions => Set<ShareTransaction>();
    public DbSet<Staff> Staffs => Set<Staff>();
    public DbSet<StaffTraining> StaffTrainings => Set<StaffTraining>();
    public DbSet<TellerSession> TellerSessions => Set<TellerSession>();
    public DbSet<UssdSession> UssdSessions => Set<UssdSession>();

    public MicrofinanceDbContext(
        DbContextOptions<MicrofinanceDbContext> options,
        IMultiTenantContextAccessor<AppTenantInfo> multiTenantContextAccessor,
        IOptions<DatabaseOptions> settings,
        IHostEnvironment environment)
        : base(options)
    {
        _settings = settings.Value;
        _environment = environment;
        TenantInfo = multiTenantContextAccessor.MultiTenantContext?.TenantInfo!;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.HasDefaultSchema("microfinance");
    }

    public string? TenantInfo_Identifier => TenantInfo?.Identifier;
    public Guid UserId => Guid.Empty;
    public string UserName => "System";
}
