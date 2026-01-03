using Asp.Versioning;
using Asp.Versioning.Builder;
using FSH.Framework.Shared.Identity;
using FSH.Framework.Web.Modules;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Features.v1.AgentBankings.CreateAgentBanking;
using FSH.Modules.Microfinance.Features.v1.AgentBankings.DeleteAgentBanking;
using FSH.Modules.Microfinance.Features.v1.AgentBankings.GetAgentBanking;
using FSH.Modules.Microfinance.Features.v1.AgentBankings.GetAgentBankings;
using FSH.Modules.Microfinance.Features.v1.AgentBankings.UpdateAgentBanking;
using FSH.Modules.Microfinance.Features.v1.AmlAlerts.CreateAmlAlert;
using FSH.Modules.Microfinance.Features.v1.AmlAlerts.DeleteAmlAlert;
using FSH.Modules.Microfinance.Features.v1.AmlAlerts.GetAmlAlert;
using FSH.Modules.Microfinance.Features.v1.AmlAlerts.GetAmlAlerts;
using FSH.Modules.Microfinance.Features.v1.AmlAlerts.UpdateAmlAlert;
using FSH.Modules.Microfinance.Features.v1.ApprovalRequests.CreateApprovalRequest;
using FSH.Modules.Microfinance.Features.v1.ApprovalRequests.DeleteApprovalRequest;
using FSH.Modules.Microfinance.Features.v1.ApprovalRequests.GetApprovalRequest;
using FSH.Modules.Microfinance.Features.v1.ApprovalRequests.GetApprovalRequests;
using FSH.Modules.Microfinance.Features.v1.ApprovalRequests.UpdateApprovalRequest;
using FSH.Modules.Microfinance.Features.v1.ApprovalWorkflows.CreateApprovalWorkflow;
using FSH.Modules.Microfinance.Features.v1.ApprovalWorkflows.DeleteApprovalWorkflow;
using FSH.Modules.Microfinance.Features.v1.ApprovalWorkflows.GetApprovalWorkflow;
using FSH.Modules.Microfinance.Features.v1.ApprovalWorkflows.GetApprovalWorkflows;
using FSH.Modules.Microfinance.Features.v1.ApprovalWorkflows.UpdateApprovalWorkflow;
using FSH.Modules.Microfinance.Features.v1.Branches.CreateBranch;
using FSH.Modules.Microfinance.Features.v1.Branches.DeleteBranch;
using FSH.Modules.Microfinance.Features.v1.Branches.GetBranch;
using FSH.Modules.Microfinance.Features.v1.Branches.GetBranchs;
using FSH.Modules.Microfinance.Features.v1.Branches.UpdateBranch;
using FSH.Modules.Microfinance.Features.v1.BranchTargets.CreateBranchTarget;
using FSH.Modules.Microfinance.Features.v1.BranchTargets.DeleteBranchTarget;
using FSH.Modules.Microfinance.Features.v1.BranchTargets.GetBranchTarget;
using FSH.Modules.Microfinance.Features.v1.BranchTargets.GetBranchTargets;
using FSH.Modules.Microfinance.Features.v1.BranchTargets.UpdateBranchTarget;
using FSH.Modules.Microfinance.Features.v1.CashVaults.CreateCashVault;
using FSH.Modules.Microfinance.Features.v1.CashVaults.DeleteCashVault;
using FSH.Modules.Microfinance.Features.v1.CashVaults.GetCashVault;
using FSH.Modules.Microfinance.Features.v1.CashVaults.GetCashVaults;
using FSH.Modules.Microfinance.Features.v1.CashVaults.UpdateCashVault;
using FSH.Modules.Microfinance.Features.v1.CollateralInsurances.CreateCollateralInsurance;
using FSH.Modules.Microfinance.Features.v1.CollateralInsurances.DeleteCollateralInsurance;
using FSH.Modules.Microfinance.Features.v1.CollateralInsurances.GetCollateralInsurance;
using FSH.Modules.Microfinance.Features.v1.CollateralInsurances.GetCollateralInsurances;
using FSH.Modules.Microfinance.Features.v1.CollateralInsurances.UpdateCollateralInsurance;
using FSH.Modules.Microfinance.Features.v1.CollateralReleases.CreateCollateralRelease;
using FSH.Modules.Microfinance.Features.v1.CollateralReleases.DeleteCollateralRelease;
using FSH.Modules.Microfinance.Features.v1.CollateralReleases.GetCollateralRelease;
using FSH.Modules.Microfinance.Features.v1.CollateralReleases.GetCollateralReleases;
using FSH.Modules.Microfinance.Features.v1.CollateralReleases.UpdateCollateralRelease;
using FSH.Modules.Microfinance.Features.v1.CollateralTypes.CreateCollateralType;
using FSH.Modules.Microfinance.Features.v1.CollateralTypes.DeleteCollateralType;
using FSH.Modules.Microfinance.Features.v1.CollateralTypes.GetCollateralType;
using FSH.Modules.Microfinance.Features.v1.CollateralTypes.GetCollateralTypes;
using FSH.Modules.Microfinance.Features.v1.CollateralTypes.UpdateCollateralType;
using FSH.Modules.Microfinance.Features.v1.CollateralValuations.CreateCollateralValuation;
using FSH.Modules.Microfinance.Features.v1.CollateralValuations.DeleteCollateralValuation;
using FSH.Modules.Microfinance.Features.v1.CollateralValuations.GetCollateralValuation;
using FSH.Modules.Microfinance.Features.v1.CollateralValuations.GetCollateralValuations;
using FSH.Modules.Microfinance.Features.v1.CollateralValuations.UpdateCollateralValuation;
using FSH.Modules.Microfinance.Features.v1.CollectionActions.CreateCollectionAction;
using FSH.Modules.Microfinance.Features.v1.CollectionActions.DeleteCollectionAction;
using FSH.Modules.Microfinance.Features.v1.CollectionActions.GetCollectionAction;
using FSH.Modules.Microfinance.Features.v1.CollectionActions.GetCollectionActions;
using FSH.Modules.Microfinance.Features.v1.CollectionActions.UpdateCollectionAction;
using FSH.Modules.Microfinance.Features.v1.CollectionCases.CreateCollectionCase;
using FSH.Modules.Microfinance.Features.v1.CollectionCases.DeleteCollectionCase;
using FSH.Modules.Microfinance.Features.v1.CollectionCases.GetCollectionCase;
using FSH.Modules.Microfinance.Features.v1.CollectionCases.GetCollectionCases;
using FSH.Modules.Microfinance.Features.v1.CollectionCases.UpdateCollectionCase;
using FSH.Modules.Microfinance.Features.v1.CollectionStrategys.CreateCollectionStrategy;
using FSH.Modules.Microfinance.Features.v1.CollectionStrategys.DeleteCollectionStrategy;
using FSH.Modules.Microfinance.Features.v1.CollectionStrategys.GetCollectionStrategy;
using FSH.Modules.Microfinance.Features.v1.CollectionStrategys.GetCollectionStrategys;
using FSH.Modules.Microfinance.Features.v1.CollectionStrategys.UpdateCollectionStrategy;
using FSH.Modules.Microfinance.Features.v1.CommunicationLogs.CreateCommunicationLog;
using FSH.Modules.Microfinance.Features.v1.CommunicationLogs.DeleteCommunicationLog;
using FSH.Modules.Microfinance.Features.v1.CommunicationLogs.GetCommunicationLog;
using FSH.Modules.Microfinance.Features.v1.CommunicationLogs.GetCommunicationLogs;
using FSH.Modules.Microfinance.Features.v1.CommunicationLogs.UpdateCommunicationLog;
using FSH.Modules.Microfinance.Features.v1.CommunicationTemplates.CreateCommunicationTemplate;
using FSH.Modules.Microfinance.Features.v1.CommunicationTemplates.DeleteCommunicationTemplate;
using FSH.Modules.Microfinance.Features.v1.CommunicationTemplates.GetCommunicationTemplate;
using FSH.Modules.Microfinance.Features.v1.CommunicationTemplates.GetCommunicationTemplates;
using FSH.Modules.Microfinance.Features.v1.CommunicationTemplates.UpdateCommunicationTemplate;
using FSH.Modules.Microfinance.Features.v1.CreditBureauInquirys.CreateCreditBureauInquiry;
using FSH.Modules.Microfinance.Features.v1.CreditBureauInquirys.DeleteCreditBureauInquiry;
using FSH.Modules.Microfinance.Features.v1.CreditBureauInquirys.GetCreditBureauInquiry;
using FSH.Modules.Microfinance.Features.v1.CreditBureauInquirys.GetCreditBureauInquirys;
using FSH.Modules.Microfinance.Features.v1.CreditBureauInquirys.UpdateCreditBureauInquiry;
using FSH.Modules.Microfinance.Features.v1.CreditBureauReports.CreateCreditBureauReport;
using FSH.Modules.Microfinance.Features.v1.CreditBureauReports.DeleteCreditBureauReport;
using FSH.Modules.Microfinance.Features.v1.CreditBureauReports.GetCreditBureauReport;
using FSH.Modules.Microfinance.Features.v1.CreditBureauReports.GetCreditBureauReports;
using FSH.Modules.Microfinance.Features.v1.CreditBureauReports.UpdateCreditBureauReport;
using FSH.Modules.Microfinance.Features.v1.CreditScores.CreateCreditScore;
using FSH.Modules.Microfinance.Features.v1.CreditScores.DeleteCreditScore;
using FSH.Modules.Microfinance.Features.v1.CreditScores.GetCreditScore;
using FSH.Modules.Microfinance.Features.v1.CreditScores.GetCreditScores;
using FSH.Modules.Microfinance.Features.v1.CreditScores.UpdateCreditScore;
using FSH.Modules.Microfinance.Features.v1.CustomerCases.CreateCustomerCase;
using FSH.Modules.Microfinance.Features.v1.CustomerCases.DeleteCustomerCase;
using FSH.Modules.Microfinance.Features.v1.CustomerCases.GetCustomerCase;
using FSH.Modules.Microfinance.Features.v1.CustomerCases.GetCustomerCases;
using FSH.Modules.Microfinance.Features.v1.CustomerCases.UpdateCustomerCase;
using FSH.Modules.Microfinance.Features.v1.CustomerSegments.CreateCustomerSegment;
using FSH.Modules.Microfinance.Features.v1.CustomerSegments.DeleteCustomerSegment;
using FSH.Modules.Microfinance.Features.v1.CustomerSegments.GetCustomerSegment;
using FSH.Modules.Microfinance.Features.v1.CustomerSegments.GetCustomerSegments;
using FSH.Modules.Microfinance.Features.v1.CustomerSegments.UpdateCustomerSegment;
using FSH.Modules.Microfinance.Features.v1.CustomerSurveys.CreateCustomerSurvey;
using FSH.Modules.Microfinance.Features.v1.CustomerSurveys.DeleteCustomerSurvey;
using FSH.Modules.Microfinance.Features.v1.CustomerSurveys.GetCustomerSurvey;
using FSH.Modules.Microfinance.Features.v1.CustomerSurveys.GetCustomerSurveys;
using FSH.Modules.Microfinance.Features.v1.CustomerSurveys.UpdateCustomerSurvey;
using FSH.Modules.Microfinance.Features.v1.DebtSettlements.CreateDebtSettlement;
using FSH.Modules.Microfinance.Features.v1.DebtSettlements.DeleteDebtSettlement;
using FSH.Modules.Microfinance.Features.v1.DebtSettlements.GetDebtSettlement;
using FSH.Modules.Microfinance.Features.v1.DebtSettlements.GetDebtSettlements;
using FSH.Modules.Microfinance.Features.v1.DebtSettlements.UpdateDebtSettlement;
using FSH.Modules.Microfinance.Features.v1.Documents.CreateDocument;
using FSH.Modules.Microfinance.Features.v1.Documents.DeleteDocument;
using FSH.Modules.Microfinance.Features.v1.Documents.GetDocument;
using FSH.Modules.Microfinance.Features.v1.Documents.GetDocuments;
using FSH.Modules.Microfinance.Features.v1.Documents.UpdateDocument;
using FSH.Modules.Microfinance.Features.v1.FeeCharges.CreateFeeCharge;
using FSH.Modules.Microfinance.Features.v1.FeeCharges.DeleteFeeCharge;
using FSH.Modules.Microfinance.Features.v1.FeeCharges.GetFeeCharge;
using FSH.Modules.Microfinance.Features.v1.FeeCharges.GetFeeCharges;
using FSH.Modules.Microfinance.Features.v1.FeeCharges.UpdateFeeCharge;
using FSH.Modules.Microfinance.Features.v1.FeeDefinitions.CreateFeeDefinition;
using FSH.Modules.Microfinance.Features.v1.FeeDefinitions.DeleteFeeDefinition;
using FSH.Modules.Microfinance.Features.v1.FeeDefinitions.GetFeeDefinition;
using FSH.Modules.Microfinance.Features.v1.FeeDefinitions.GetFeeDefinitions;
using FSH.Modules.Microfinance.Features.v1.FeeDefinitions.UpdateFeeDefinition;
using FSH.Modules.Microfinance.Features.v1.FeePayments.CreateFeePayment;
using FSH.Modules.Microfinance.Features.v1.FeePayments.DeleteFeePayment;
using FSH.Modules.Microfinance.Features.v1.FeePayments.GetFeePayment;
using FSH.Modules.Microfinance.Features.v1.FeePayments.GetFeePayments;
using FSH.Modules.Microfinance.Features.v1.FeePayments.UpdateFeePayment;
using FSH.Modules.Microfinance.Features.v1.FeeWaivers.CreateFeeWaiver;
using FSH.Modules.Microfinance.Features.v1.FeeWaivers.DeleteFeeWaiver;
using FSH.Modules.Microfinance.Features.v1.FeeWaivers.GetFeeWaiver;
using FSH.Modules.Microfinance.Features.v1.FeeWaivers.GetFeeWaivers;
using FSH.Modules.Microfinance.Features.v1.FeeWaivers.UpdateFeeWaiver;
using FSH.Modules.Microfinance.Features.v1.FixedDeposits.CreateFixedDeposit;
using FSH.Modules.Microfinance.Features.v1.FixedDeposits.DeleteFixedDeposit;
using FSH.Modules.Microfinance.Features.v1.FixedDeposits.GetFixedDeposit;
using FSH.Modules.Microfinance.Features.v1.FixedDeposits.GetFixedDeposits;
using FSH.Modules.Microfinance.Features.v1.FixedDeposits.UpdateFixedDeposit;
using FSH.Modules.Microfinance.Features.v1.GroupMemberships.CreateGroupMembership;
using FSH.Modules.Microfinance.Features.v1.GroupMemberships.DeleteGroupMembership;
using FSH.Modules.Microfinance.Features.v1.GroupMemberships.GetGroupMembership;
using FSH.Modules.Microfinance.Features.v1.GroupMemberships.GetGroupMemberships;
using FSH.Modules.Microfinance.Features.v1.GroupMemberships.UpdateGroupMembership;
using FSH.Modules.Microfinance.Features.v1.InsuranceClaims.CreateInsuranceClaim;
using FSH.Modules.Microfinance.Features.v1.InsuranceClaims.DeleteInsuranceClaim;
using FSH.Modules.Microfinance.Features.v1.InsuranceClaims.GetInsuranceClaim;
using FSH.Modules.Microfinance.Features.v1.InsuranceClaims.GetInsuranceClaims;
using FSH.Modules.Microfinance.Features.v1.InsuranceClaims.UpdateInsuranceClaim;
using FSH.Modules.Microfinance.Features.v1.InsurancePolicys.CreateInsurancePolicy;
using FSH.Modules.Microfinance.Features.v1.InsurancePolicys.DeleteInsurancePolicy;
using FSH.Modules.Microfinance.Features.v1.InsurancePolicys.GetInsurancePolicy;
using FSH.Modules.Microfinance.Features.v1.InsurancePolicys.GetInsurancePolicys;
using FSH.Modules.Microfinance.Features.v1.InsurancePolicys.UpdateInsurancePolicy;
using FSH.Modules.Microfinance.Features.v1.InsuranceProducts.CreateInsuranceProduct;
using FSH.Modules.Microfinance.Features.v1.InsuranceProducts.DeleteInsuranceProduct;
using FSH.Modules.Microfinance.Features.v1.InsuranceProducts.GetInsuranceProduct;
using FSH.Modules.Microfinance.Features.v1.InsuranceProducts.GetInsuranceProducts;
using FSH.Modules.Microfinance.Features.v1.InsuranceProducts.UpdateInsuranceProduct;
using FSH.Modules.Microfinance.Features.v1.InterestRateChanges.CreateInterestRateChange;
using FSH.Modules.Microfinance.Features.v1.InterestRateChanges.DeleteInterestRateChange;
using FSH.Modules.Microfinance.Features.v1.InterestRateChanges.GetInterestRateChange;
using FSH.Modules.Microfinance.Features.v1.InterestRateChanges.GetInterestRateChanges;
using FSH.Modules.Microfinance.Features.v1.InterestRateChanges.UpdateInterestRateChange;
using FSH.Modules.Microfinance.Features.v1.InvestmentAccounts.CreateInvestmentAccount;
using FSH.Modules.Microfinance.Features.v1.InvestmentAccounts.DeleteInvestmentAccount;
using FSH.Modules.Microfinance.Features.v1.InvestmentAccounts.GetInvestmentAccount;
using FSH.Modules.Microfinance.Features.v1.InvestmentAccounts.GetInvestmentAccounts;
using FSH.Modules.Microfinance.Features.v1.InvestmentAccounts.UpdateInvestmentAccount;
using FSH.Modules.Microfinance.Features.v1.InvestmentProducts.CreateInvestmentProduct;
using FSH.Modules.Microfinance.Features.v1.InvestmentProducts.DeleteInvestmentProduct;
using FSH.Modules.Microfinance.Features.v1.InvestmentProducts.GetInvestmentProduct;
using FSH.Modules.Microfinance.Features.v1.InvestmentProducts.GetInvestmentProducts;
using FSH.Modules.Microfinance.Features.v1.InvestmentProducts.UpdateInvestmentProduct;
using FSH.Modules.Microfinance.Features.v1.InvestmentTransactions.CreateInvestmentTransaction;
using FSH.Modules.Microfinance.Features.v1.InvestmentTransactions.DeleteInvestmentTransaction;
using FSH.Modules.Microfinance.Features.v1.InvestmentTransactions.GetInvestmentTransaction;
using FSH.Modules.Microfinance.Features.v1.InvestmentTransactions.GetInvestmentTransactions;
using FSH.Modules.Microfinance.Features.v1.InvestmentTransactions.UpdateInvestmentTransaction;
using FSH.Modules.Microfinance.Features.v1.KycDocuments.CreateKycDocument;
using FSH.Modules.Microfinance.Features.v1.KycDocuments.DeleteKycDocument;
using FSH.Modules.Microfinance.Features.v1.KycDocuments.GetKycDocument;
using FSH.Modules.Microfinance.Features.v1.KycDocuments.GetKycDocuments;
using FSH.Modules.Microfinance.Features.v1.KycDocuments.UpdateKycDocument;
using FSH.Modules.Microfinance.Features.v1.LegalActions.CreateLegalAction;
using FSH.Modules.Microfinance.Features.v1.LegalActions.DeleteLegalAction;
using FSH.Modules.Microfinance.Features.v1.LegalActions.GetLegalAction;
using FSH.Modules.Microfinance.Features.v1.LegalActions.GetLegalActions;
using FSH.Modules.Microfinance.Features.v1.LegalActions.UpdateLegalAction;
using FSH.Modules.Microfinance.Features.v1.LoanApplications.CreateLoanApplication;
using FSH.Modules.Microfinance.Features.v1.LoanApplications.DeleteLoanApplication;
using FSH.Modules.Microfinance.Features.v1.LoanApplications.GetLoanApplication;
using FSH.Modules.Microfinance.Features.v1.LoanApplications.GetLoanApplications;
using FSH.Modules.Microfinance.Features.v1.LoanApplications.UpdateLoanApplication;
using FSH.Modules.Microfinance.Features.v1.LoanCollaterals.CreateLoanCollateral;
using FSH.Modules.Microfinance.Features.v1.LoanCollaterals.DeleteLoanCollateral;
using FSH.Modules.Microfinance.Features.v1.LoanCollaterals.GetLoanCollateral;
using FSH.Modules.Microfinance.Features.v1.LoanCollaterals.GetLoanCollaterals;
using FSH.Modules.Microfinance.Features.v1.LoanCollaterals.UpdateLoanCollateral;
using FSH.Modules.Microfinance.Features.v1.LoanDisbursementTranches.CreateLoanDisbursementTranche;
using FSH.Modules.Microfinance.Features.v1.LoanDisbursementTranches.DeleteLoanDisbursementTranche;
using FSH.Modules.Microfinance.Features.v1.LoanDisbursementTranches.GetLoanDisbursementTranche;
using FSH.Modules.Microfinance.Features.v1.LoanDisbursementTranches.GetLoanDisbursementTranches;
using FSH.Modules.Microfinance.Features.v1.LoanDisbursementTranches.UpdateLoanDisbursementTranche;
using FSH.Modules.Microfinance.Features.v1.LoanGuarantors.CreateLoanGuarantor;
using FSH.Modules.Microfinance.Features.v1.LoanGuarantors.DeleteLoanGuarantor;
using FSH.Modules.Microfinance.Features.v1.LoanGuarantors.GetLoanGuarantor;
using FSH.Modules.Microfinance.Features.v1.LoanGuarantors.GetLoanGuarantors;
using FSH.Modules.Microfinance.Features.v1.LoanGuarantors.UpdateLoanGuarantor;
using FSH.Modules.Microfinance.Features.v1.LoanOfficerAssignments.CreateLoanOfficerAssignment;
using FSH.Modules.Microfinance.Features.v1.LoanOfficerAssignments.DeleteLoanOfficerAssignment;
using FSH.Modules.Microfinance.Features.v1.LoanOfficerAssignments.GetLoanOfficerAssignment;
using FSH.Modules.Microfinance.Features.v1.LoanOfficerAssignments.GetLoanOfficerAssignments;
using FSH.Modules.Microfinance.Features.v1.LoanOfficerAssignments.UpdateLoanOfficerAssignment;
using FSH.Modules.Microfinance.Features.v1.LoanOfficerTargets.CreateLoanOfficerTarget;
using FSH.Modules.Microfinance.Features.v1.LoanOfficerTargets.DeleteLoanOfficerTarget;
using FSH.Modules.Microfinance.Features.v1.LoanOfficerTargets.GetLoanOfficerTarget;
using FSH.Modules.Microfinance.Features.v1.LoanOfficerTargets.GetLoanOfficerTargets;
using FSH.Modules.Microfinance.Features.v1.LoanOfficerTargets.UpdateLoanOfficerTarget;
using FSH.Modules.Microfinance.Features.v1.LoanProducts.CreateLoanProduct;
using FSH.Modules.Microfinance.Features.v1.LoanProducts.DeleteLoanProduct;
using FSH.Modules.Microfinance.Features.v1.LoanProducts.GetLoanProduct;
using FSH.Modules.Microfinance.Features.v1.LoanProducts.GetLoanProducts;
using FSH.Modules.Microfinance.Features.v1.LoanProducts.UpdateLoanProduct;
using FSH.Modules.Microfinance.Features.v1.LoanRepayments.CreateLoanRepayment;
using FSH.Modules.Microfinance.Features.v1.LoanRepayments.DeleteLoanRepayment;
using FSH.Modules.Microfinance.Features.v1.LoanRepayments.GetLoanRepayment;
using FSH.Modules.Microfinance.Features.v1.LoanRepayments.GetLoanRepayments;
using FSH.Modules.Microfinance.Features.v1.LoanRepayments.UpdateLoanRepayment;
using FSH.Modules.Microfinance.Features.v1.LoanRestructures.CreateLoanRestructure;
using FSH.Modules.Microfinance.Features.v1.LoanRestructures.DeleteLoanRestructure;
using FSH.Modules.Microfinance.Features.v1.LoanRestructures.GetLoanRestructure;
using FSH.Modules.Microfinance.Features.v1.LoanRestructures.GetLoanRestructures;
using FSH.Modules.Microfinance.Features.v1.LoanRestructures.UpdateLoanRestructure;
using FSH.Modules.Microfinance.Features.v1.Loans.CreateLoan;
using FSH.Modules.Microfinance.Features.v1.Loans.DeleteLoan;
using FSH.Modules.Microfinance.Features.v1.Loans.GetLoan;
using FSH.Modules.Microfinance.Features.v1.Loans.GetLoans;
using FSH.Modules.Microfinance.Features.v1.Loans.UpdateLoan;
using FSH.Modules.Microfinance.Features.v1.LoanSchedules.CreateLoanSchedule;
using FSH.Modules.Microfinance.Features.v1.LoanSchedules.DeleteLoanSchedule;
using FSH.Modules.Microfinance.Features.v1.LoanSchedules.GetLoanSchedule;
using FSH.Modules.Microfinance.Features.v1.LoanSchedules.GetLoanSchedules;
using FSH.Modules.Microfinance.Features.v1.LoanSchedules.UpdateLoanSchedule;
using FSH.Modules.Microfinance.Features.v1.LoanWriteOffs.CreateLoanWriteOff;
using FSH.Modules.Microfinance.Features.v1.LoanWriteOffs.DeleteLoanWriteOff;
using FSH.Modules.Microfinance.Features.v1.LoanWriteOffs.GetLoanWriteOff;
using FSH.Modules.Microfinance.Features.v1.LoanWriteOffs.GetLoanWriteOffs;
using FSH.Modules.Microfinance.Features.v1.LoanWriteOffs.UpdateLoanWriteOff;
using FSH.Modules.Microfinance.Features.v1.MarketingCampaigns.CreateMarketingCampaign;
using FSH.Modules.Microfinance.Features.v1.MarketingCampaigns.DeleteMarketingCampaign;
using FSH.Modules.Microfinance.Features.v1.MarketingCampaigns.GetMarketingCampaign;
using FSH.Modules.Microfinance.Features.v1.MarketingCampaigns.GetMarketingCampaigns;
using FSH.Modules.Microfinance.Features.v1.MarketingCampaigns.UpdateMarketingCampaign;
using FSH.Modules.Microfinance.Features.v1.MemberGroups.CreateMemberGroup;
using FSH.Modules.Microfinance.Features.v1.MemberGroups.DeleteMemberGroup;
using FSH.Modules.Microfinance.Features.v1.MemberGroups.GetMemberGroup;
using FSH.Modules.Microfinance.Features.v1.MemberGroups.GetMemberGroups;
using FSH.Modules.Microfinance.Features.v1.MemberGroups.UpdateMemberGroup;
using FSH.Modules.Microfinance.Features.v1.Members.CreateMember;
using FSH.Modules.Microfinance.Features.v1.Members.DeleteMember;
using FSH.Modules.Microfinance.Features.v1.Members.GetMember;
using FSH.Modules.Microfinance.Features.v1.Members.GetMembers;
using FSH.Modules.Microfinance.Features.v1.Members.UpdateMember;
using FSH.Modules.Microfinance.Features.v1.MfiConfigurations.CreateMfiConfiguration;
using FSH.Modules.Microfinance.Features.v1.MfiConfigurations.DeleteMfiConfiguration;
using FSH.Modules.Microfinance.Features.v1.MfiConfigurations.GetMfiConfiguration;
using FSH.Modules.Microfinance.Features.v1.MfiConfigurations.GetMfiConfigurations;
using FSH.Modules.Microfinance.Features.v1.MfiConfigurations.UpdateMfiConfiguration;
using FSH.Modules.Microfinance.Features.v1.MobileTransactions.CreateMobileTransaction;
using FSH.Modules.Microfinance.Features.v1.MobileTransactions.DeleteMobileTransaction;
using FSH.Modules.Microfinance.Features.v1.MobileTransactions.GetMobileTransaction;
using FSH.Modules.Microfinance.Features.v1.MobileTransactions.GetMobileTransactions;
using FSH.Modules.Microfinance.Features.v1.MobileTransactions.UpdateMobileTransaction;
using FSH.Modules.Microfinance.Features.v1.MobileWallets.CreateMobileWallet;
using FSH.Modules.Microfinance.Features.v1.MobileWallets.DeleteMobileWallet;
using FSH.Modules.Microfinance.Features.v1.MobileWallets.GetMobileWallet;
using FSH.Modules.Microfinance.Features.v1.MobileWallets.GetMobileWallets;
using FSH.Modules.Microfinance.Features.v1.MobileWallets.UpdateMobileWallet;
using FSH.Modules.Microfinance.Features.v1.PaymentGateways.CreatePaymentGateway;
using FSH.Modules.Microfinance.Features.v1.PaymentGateways.DeletePaymentGateway;
using FSH.Modules.Microfinance.Features.v1.PaymentGateways.GetPaymentGateway;
using FSH.Modules.Microfinance.Features.v1.PaymentGateways.GetPaymentGateways;
using FSH.Modules.Microfinance.Features.v1.PaymentGateways.UpdatePaymentGateway;
using FSH.Modules.Microfinance.Features.v1.PromiseToPays.CreatePromiseToPay;
using FSH.Modules.Microfinance.Features.v1.PromiseToPays.DeletePromiseToPay;
using FSH.Modules.Microfinance.Features.v1.PromiseToPays.GetPromiseToPay;
using FSH.Modules.Microfinance.Features.v1.PromiseToPays.GetPromiseToPays;
using FSH.Modules.Microfinance.Features.v1.PromiseToPays.UpdatePromiseToPay;
using FSH.Modules.Microfinance.Features.v1.QrPayments.CreateQrPayment;
using FSH.Modules.Microfinance.Features.v1.QrPayments.DeleteQrPayment;
using FSH.Modules.Microfinance.Features.v1.QrPayments.GetQrPayment;
using FSH.Modules.Microfinance.Features.v1.QrPayments.GetQrPayments;
using FSH.Modules.Microfinance.Features.v1.QrPayments.UpdateQrPayment;
using FSH.Modules.Microfinance.Features.v1.ReportDefinitions.CreateReportDefinition;
using FSH.Modules.Microfinance.Features.v1.ReportDefinitions.DeleteReportDefinition;
using FSH.Modules.Microfinance.Features.v1.ReportDefinitions.GetReportDefinition;
using FSH.Modules.Microfinance.Features.v1.ReportDefinitions.GetReportDefinitions;
using FSH.Modules.Microfinance.Features.v1.ReportDefinitions.UpdateReportDefinition;
using FSH.Modules.Microfinance.Features.v1.ReportGenerations.CreateReportGeneration;
using FSH.Modules.Microfinance.Features.v1.ReportGenerations.DeleteReportGeneration;
using FSH.Modules.Microfinance.Features.v1.ReportGenerations.GetReportGeneration;
using FSH.Modules.Microfinance.Features.v1.ReportGenerations.GetReportGenerations;
using FSH.Modules.Microfinance.Features.v1.ReportGenerations.UpdateReportGeneration;
using FSH.Modules.Microfinance.Features.v1.RiskAlerts.CreateRiskAlert;
using FSH.Modules.Microfinance.Features.v1.RiskAlerts.DeleteRiskAlert;
using FSH.Modules.Microfinance.Features.v1.RiskAlerts.GetRiskAlert;
using FSH.Modules.Microfinance.Features.v1.RiskAlerts.GetRiskAlerts;
using FSH.Modules.Microfinance.Features.v1.RiskAlerts.UpdateRiskAlert;
using FSH.Modules.Microfinance.Features.v1.RiskCategorys.CreateRiskCategory;
using FSH.Modules.Microfinance.Features.v1.RiskCategorys.DeleteRiskCategory;
using FSH.Modules.Microfinance.Features.v1.RiskCategorys.GetRiskCategory;
using FSH.Modules.Microfinance.Features.v1.RiskCategorys.GetRiskCategorys;
using FSH.Modules.Microfinance.Features.v1.RiskCategorys.UpdateRiskCategory;
using FSH.Modules.Microfinance.Features.v1.RiskIndicators.CreateRiskIndicator;
using FSH.Modules.Microfinance.Features.v1.RiskIndicators.DeleteRiskIndicator;
using FSH.Modules.Microfinance.Features.v1.RiskIndicators.GetRiskIndicator;
using FSH.Modules.Microfinance.Features.v1.RiskIndicators.GetRiskIndicators;
using FSH.Modules.Microfinance.Features.v1.RiskIndicators.UpdateRiskIndicator;
using FSH.Modules.Microfinance.Features.v1.SavingsAccounts.CreateSavingsAccount;
using FSH.Modules.Microfinance.Features.v1.SavingsAccounts.DeleteSavingsAccount;
using FSH.Modules.Microfinance.Features.v1.SavingsAccounts.GetSavingsAccount;
using FSH.Modules.Microfinance.Features.v1.SavingsAccounts.GetSavingsAccounts;
using FSH.Modules.Microfinance.Features.v1.SavingsAccounts.UpdateSavingsAccount;
using FSH.Modules.Microfinance.Features.v1.SavingsProducts.CreateSavingsProduct;
using FSH.Modules.Microfinance.Features.v1.SavingsProducts.DeleteSavingsProduct;
using FSH.Modules.Microfinance.Features.v1.SavingsProducts.GetSavingsProduct;
using FSH.Modules.Microfinance.Features.v1.SavingsProducts.GetSavingsProducts;
using FSH.Modules.Microfinance.Features.v1.SavingsProducts.UpdateSavingsProduct;
using FSH.Modules.Microfinance.Features.v1.SavingsTransactions.CreateSavingsTransaction;
using FSH.Modules.Microfinance.Features.v1.SavingsTransactions.DeleteSavingsTransaction;
using FSH.Modules.Microfinance.Features.v1.SavingsTransactions.GetSavingsTransaction;
using FSH.Modules.Microfinance.Features.v1.SavingsTransactions.GetSavingsTransactions;
using FSH.Modules.Microfinance.Features.v1.SavingsTransactions.UpdateSavingsTransaction;
using FSH.Modules.Microfinance.Features.v1.ShareAccounts.CreateShareAccount;
using FSH.Modules.Microfinance.Features.v1.ShareAccounts.DeleteShareAccount;
using FSH.Modules.Microfinance.Features.v1.ShareAccounts.GetShareAccount;
using FSH.Modules.Microfinance.Features.v1.ShareAccounts.GetShareAccounts;
using FSH.Modules.Microfinance.Features.v1.ShareAccounts.UpdateShareAccount;
using FSH.Modules.Microfinance.Features.v1.ShareProducts.CreateShareProduct;
using FSH.Modules.Microfinance.Features.v1.ShareProducts.DeleteShareProduct;
using FSH.Modules.Microfinance.Features.v1.ShareProducts.GetShareProduct;
using FSH.Modules.Microfinance.Features.v1.ShareProducts.GetShareProducts;
using FSH.Modules.Microfinance.Features.v1.ShareProducts.UpdateShareProduct;
using FSH.Modules.Microfinance.Features.v1.ShareTransactions.CreateShareTransaction;
using FSH.Modules.Microfinance.Features.v1.ShareTransactions.DeleteShareTransaction;
using FSH.Modules.Microfinance.Features.v1.ShareTransactions.GetShareTransaction;
using FSH.Modules.Microfinance.Features.v1.ShareTransactions.GetShareTransactions;
using FSH.Modules.Microfinance.Features.v1.ShareTransactions.UpdateShareTransaction;
using FSH.Modules.Microfinance.Features.v1.Staffs.CreateStaff;
using FSH.Modules.Microfinance.Features.v1.Staffs.DeleteStaff;
using FSH.Modules.Microfinance.Features.v1.Staffs.GetStaff;
using FSH.Modules.Microfinance.Features.v1.Staffs.GetStaffs;
using FSH.Modules.Microfinance.Features.v1.Staffs.UpdateStaff;
using FSH.Modules.Microfinance.Features.v1.StaffTrainings.CreateStaffTraining;
using FSH.Modules.Microfinance.Features.v1.StaffTrainings.DeleteStaffTraining;
using FSH.Modules.Microfinance.Features.v1.StaffTrainings.GetStaffTraining;
using FSH.Modules.Microfinance.Features.v1.StaffTrainings.GetStaffTrainings;
using FSH.Modules.Microfinance.Features.v1.StaffTrainings.UpdateStaffTraining;
using FSH.Modules.Microfinance.Features.v1.TellerSessions.CreateTellerSession;
using FSH.Modules.Microfinance.Features.v1.TellerSessions.DeleteTellerSession;
using FSH.Modules.Microfinance.Features.v1.TellerSessions.GetTellerSession;
using FSH.Modules.Microfinance.Features.v1.TellerSessions.GetTellerSessions;
using FSH.Modules.Microfinance.Features.v1.TellerSessions.UpdateTellerSession;
using FSH.Modules.Microfinance.Features.v1.UssdSessions.CreateUssdSession;
using FSH.Modules.Microfinance.Features.v1.UssdSessions.DeleteUssdSession;
using FSH.Modules.Microfinance.Features.v1.UssdSessions.GetUssdSession;
using FSH.Modules.Microfinance.Features.v1.UssdSessions.GetUssdSessions;
using FSH.Modules.Microfinance.Features.v1.UssdSessions.UpdateUssdSession;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace FSH.Modules.Microfinance;

public class MicrofinanceModule : IModule
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        // Register permissions
        PermissionConstants.Register(MicrofinancePermissionConstants.GetPermissions());

        // Register DbContext
        builder.Services.AddDbContext<MicrofinanceDbContext>();

        // Register Db Initializer
        builder.Services.AddScoped<MicrofinanceDbInitializer>();

        // Health checks
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<MicrofinanceDbContext>(
                name: "db:microfinance",
                failureStatus: HealthStatus.Unhealthy);
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        ApiVersionSet apiVersionSet = endpoints.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        // Members endpoints
        RouteGroupBuilder memberGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/members")
            .WithTags("Members")
            .WithApiVersionSet(apiVersionSet);

        memberGroup.MapCreateMemberEndpoint();
        memberGroup.MapGetMembersEndpoint();
        memberGroup.MapGetMemberEndpoint();
        memberGroup.MapUpdateMemberEndpoint();
        memberGroup.MapDeleteMemberEndpoint();
        // AgentBankings endpoints
        RouteGroupBuilder agentbankingGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/agentbankings")
            .WithTags("AgentBankings")
            .WithApiVersionSet(apiVersionSet);

        agentbankingGroup.MapCreateAgentBankingEndpoint();
        agentbankingGroup.MapGetAgentBankingsEndpoint();
        agentbankingGroup.MapGetAgentBankingEndpoint();
        agentbankingGroup.MapUpdateAgentBankingEndpoint();
        agentbankingGroup.MapDeleteAgentBankingEndpoint();
        // AmlAlerts endpoints
        RouteGroupBuilder amlalertGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/amlalerts")
            .WithTags("AmlAlerts")
            .WithApiVersionSet(apiVersionSet);

        amlalertGroup.MapCreateAmlAlertEndpoint();
        amlalertGroup.MapGetAmlAlertsEndpoint();
        amlalertGroup.MapGetAmlAlertEndpoint();
        amlalertGroup.MapUpdateAmlAlertEndpoint();
        amlalertGroup.MapDeleteAmlAlertEndpoint();
        // ApprovalRequests endpoints
        RouteGroupBuilder approvalrequestGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/approvalrequests")
            .WithTags("ApprovalRequests")
            .WithApiVersionSet(apiVersionSet);

        approvalrequestGroup.MapCreateApprovalRequestEndpoint();
        approvalrequestGroup.MapGetApprovalRequestsEndpoint();
        approvalrequestGroup.MapGetApprovalRequestEndpoint();
        approvalrequestGroup.MapUpdateApprovalRequestEndpoint();
        approvalrequestGroup.MapDeleteApprovalRequestEndpoint();
        // ApprovalWorkflows endpoints
        RouteGroupBuilder approvalworkflowGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/approvalworkflows")
            .WithTags("ApprovalWorkflows")
            .WithApiVersionSet(apiVersionSet);

        approvalworkflowGroup.MapCreateApprovalWorkflowEndpoint();
        approvalworkflowGroup.MapGetApprovalWorkflowsEndpoint();
        approvalworkflowGroup.MapGetApprovalWorkflowEndpoint();
        approvalworkflowGroup.MapUpdateApprovalWorkflowEndpoint();
        approvalworkflowGroup.MapDeleteApprovalWorkflowEndpoint();
        // Branchs endpoints
        RouteGroupBuilder branchGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/branchs")
            .WithTags("Branchs")
            .WithApiVersionSet(apiVersionSet);

        branchGroup.MapCreateBranchEndpoint();
        branchGroup.MapGetBranchsEndpoint();
        branchGroup.MapGetBranchEndpoint();
        branchGroup.MapUpdateBranchEndpoint();
        branchGroup.MapDeleteBranchEndpoint();
        // BranchTargets endpoints
        RouteGroupBuilder branchtargetGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/branchtargets")
            .WithTags("BranchTargets")
            .WithApiVersionSet(apiVersionSet);

        branchtargetGroup.MapCreateBranchTargetEndpoint();
        branchtargetGroup.MapGetBranchTargetsEndpoint();
        branchtargetGroup.MapGetBranchTargetEndpoint();
        branchtargetGroup.MapUpdateBranchTargetEndpoint();
        branchtargetGroup.MapDeleteBranchTargetEndpoint();
        // CashVaults endpoints
        RouteGroupBuilder cashvaultGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/cashvaults")
            .WithTags("CashVaults")
            .WithApiVersionSet(apiVersionSet);

        cashvaultGroup.MapCreateCashVaultEndpoint();
        cashvaultGroup.MapGetCashVaultsEndpoint();
        cashvaultGroup.MapGetCashVaultEndpoint();
        cashvaultGroup.MapUpdateCashVaultEndpoint();
        cashvaultGroup.MapDeleteCashVaultEndpoint();
        // CollateralInsurances endpoints
        RouteGroupBuilder collateralinsuranceGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/collateralinsurances")
            .WithTags("CollateralInsurances")
            .WithApiVersionSet(apiVersionSet);

        collateralinsuranceGroup.MapCreateCollateralInsuranceEndpoint();
        collateralinsuranceGroup.MapGetCollateralInsurancesEndpoint();
        collateralinsuranceGroup.MapGetCollateralInsuranceEndpoint();
        collateralinsuranceGroup.MapUpdateCollateralInsuranceEndpoint();
        collateralinsuranceGroup.MapDeleteCollateralInsuranceEndpoint();
        // CollateralReleases endpoints
        RouteGroupBuilder collateralreleaseGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/collateralreleases")
            .WithTags("CollateralReleases")
            .WithApiVersionSet(apiVersionSet);

        collateralreleaseGroup.MapCreateCollateralReleaseEndpoint();
        collateralreleaseGroup.MapGetCollateralReleasesEndpoint();
        collateralreleaseGroup.MapGetCollateralReleaseEndpoint();
        collateralreleaseGroup.MapUpdateCollateralReleaseEndpoint();
        collateralreleaseGroup.MapDeleteCollateralReleaseEndpoint();
        // CollateralTypes endpoints
        RouteGroupBuilder collateraltypeGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/collateraltypes")
            .WithTags("CollateralTypes")
            .WithApiVersionSet(apiVersionSet);

        collateraltypeGroup.MapCreateCollateralTypeEndpoint();
        collateraltypeGroup.MapGetCollateralTypesEndpoint();
        collateraltypeGroup.MapGetCollateralTypeEndpoint();
        collateraltypeGroup.MapUpdateCollateralTypeEndpoint();
        collateraltypeGroup.MapDeleteCollateralTypeEndpoint();
        // CollateralValuations endpoints
        RouteGroupBuilder collateralvaluationGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/collateralvaluations")
            .WithTags("CollateralValuations")
            .WithApiVersionSet(apiVersionSet);

        collateralvaluationGroup.MapCreateCollateralValuationEndpoint();
        collateralvaluationGroup.MapGetCollateralValuationsEndpoint();
        collateralvaluationGroup.MapGetCollateralValuationEndpoint();
        collateralvaluationGroup.MapUpdateCollateralValuationEndpoint();
        collateralvaluationGroup.MapDeleteCollateralValuationEndpoint();
        // CollectionActions endpoints
        RouteGroupBuilder collectionactionGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/collectionactions")
            .WithTags("CollectionActions")
            .WithApiVersionSet(apiVersionSet);

        collectionactionGroup.MapCreateCollectionActionEndpoint();
        collectionactionGroup.MapGetCollectionActionsEndpoint();
        collectionactionGroup.MapGetCollectionActionEndpoint();
        collectionactionGroup.MapUpdateCollectionActionEndpoint();
        collectionactionGroup.MapDeleteCollectionActionEndpoint();
        // CollectionCases endpoints
        RouteGroupBuilder collectioncaseGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/collectioncases")
            .WithTags("CollectionCases")
            .WithApiVersionSet(apiVersionSet);

        collectioncaseGroup.MapCreateCollectionCaseEndpoint();
        collectioncaseGroup.MapGetCollectionCasesEndpoint();
        collectioncaseGroup.MapGetCollectionCaseEndpoint();
        collectioncaseGroup.MapUpdateCollectionCaseEndpoint();
        collectioncaseGroup.MapDeleteCollectionCaseEndpoint();
        // CollectionStrategys endpoints
        RouteGroupBuilder collectionstrategyGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/collectionstrategys")
            .WithTags("CollectionStrategys")
            .WithApiVersionSet(apiVersionSet);

        collectionstrategyGroup.MapCreateCollectionStrategyEndpoint();
        collectionstrategyGroup.MapGetCollectionStrategysEndpoint();
        collectionstrategyGroup.MapGetCollectionStrategyEndpoint();
        collectionstrategyGroup.MapUpdateCollectionStrategyEndpoint();
        collectionstrategyGroup.MapDeleteCollectionStrategyEndpoint();
        // CommunicationLogs endpoints
        RouteGroupBuilder communicationlogGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/communicationlogs")
            .WithTags("CommunicationLogs")
            .WithApiVersionSet(apiVersionSet);

        communicationlogGroup.MapCreateCommunicationLogEndpoint();
        communicationlogGroup.MapGetCommunicationLogsEndpoint();
        communicationlogGroup.MapGetCommunicationLogEndpoint();
        communicationlogGroup.MapUpdateCommunicationLogEndpoint();
        communicationlogGroup.MapDeleteCommunicationLogEndpoint();
        // CommunicationTemplates endpoints
        RouteGroupBuilder communicationtemplateGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/communicationtemplates")
            .WithTags("CommunicationTemplates")
            .WithApiVersionSet(apiVersionSet);

        communicationtemplateGroup.MapCreateCommunicationTemplateEndpoint();
        communicationtemplateGroup.MapGetCommunicationTemplatesEndpoint();
        communicationtemplateGroup.MapGetCommunicationTemplateEndpoint();
        communicationtemplateGroup.MapUpdateCommunicationTemplateEndpoint();
        communicationtemplateGroup.MapDeleteCommunicationTemplateEndpoint();
        // CreditBureauInquirys endpoints
        RouteGroupBuilder creditbureauinquiryGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/creditbureauinquirys")
            .WithTags("CreditBureauInquirys")
            .WithApiVersionSet(apiVersionSet);

        creditbureauinquiryGroup.MapCreateCreditBureauInquiryEndpoint();
        creditbureauinquiryGroup.MapGetCreditBureauInquirysEndpoint();
        creditbureauinquiryGroup.MapGetCreditBureauInquiryEndpoint();
        creditbureauinquiryGroup.MapUpdateCreditBureauInquiryEndpoint();
        creditbureauinquiryGroup.MapDeleteCreditBureauInquiryEndpoint();
        // CreditBureauReports endpoints
        RouteGroupBuilder creditbureaureportGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/creditbureaureports")
            .WithTags("CreditBureauReports")
            .WithApiVersionSet(apiVersionSet);

        creditbureaureportGroup.MapCreateCreditBureauReportEndpoint();
        creditbureaureportGroup.MapGetCreditBureauReportsEndpoint();
        creditbureaureportGroup.MapGetCreditBureauReportEndpoint();
        creditbureaureportGroup.MapUpdateCreditBureauReportEndpoint();
        creditbureaureportGroup.MapDeleteCreditBureauReportEndpoint();
        // CreditScores endpoints
        RouteGroupBuilder creditscoreGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/creditscores")
            .WithTags("CreditScores")
            .WithApiVersionSet(apiVersionSet);

        creditscoreGroup.MapCreateCreditScoreEndpoint();
        creditscoreGroup.MapGetCreditScoresEndpoint();
        creditscoreGroup.MapGetCreditScoreEndpoint();
        creditscoreGroup.MapUpdateCreditScoreEndpoint();
        creditscoreGroup.MapDeleteCreditScoreEndpoint();
        // CustomerCases endpoints
        RouteGroupBuilder customercaseGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/customercases")
            .WithTags("CustomerCases")
            .WithApiVersionSet(apiVersionSet);

        customercaseGroup.MapCreateCustomerCaseEndpoint();
        customercaseGroup.MapGetCustomerCasesEndpoint();
        customercaseGroup.MapGetCustomerCaseEndpoint();
        customercaseGroup.MapUpdateCustomerCaseEndpoint();
        customercaseGroup.MapDeleteCustomerCaseEndpoint();
        // CustomerSegments endpoints
        RouteGroupBuilder customersegmentGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/customersegments")
            .WithTags("CustomerSegments")
            .WithApiVersionSet(apiVersionSet);

        customersegmentGroup.MapCreateCustomerSegmentEndpoint();
        customersegmentGroup.MapGetCustomerSegmentsEndpoint();
        customersegmentGroup.MapGetCustomerSegmentEndpoint();
        customersegmentGroup.MapUpdateCustomerSegmentEndpoint();
        customersegmentGroup.MapDeleteCustomerSegmentEndpoint();
        // CustomerSurveys endpoints
        RouteGroupBuilder customersurveyGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/customersurveys")
            .WithTags("CustomerSurveys")
            .WithApiVersionSet(apiVersionSet);

        customersurveyGroup.MapCreateCustomerSurveyEndpoint();
        customersurveyGroup.MapGetCustomerSurveysEndpoint();
        customersurveyGroup.MapGetCustomerSurveyEndpoint();
        customersurveyGroup.MapUpdateCustomerSurveyEndpoint();
        customersurveyGroup.MapDeleteCustomerSurveyEndpoint();
        // DebtSettlements endpoints
        RouteGroupBuilder debtsettlementGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/debtsettlements")
            .WithTags("DebtSettlements")
            .WithApiVersionSet(apiVersionSet);

        debtsettlementGroup.MapCreateDebtSettlementEndpoint();
        debtsettlementGroup.MapGetDebtSettlementsEndpoint();
        debtsettlementGroup.MapGetDebtSettlementEndpoint();
        debtsettlementGroup.MapUpdateDebtSettlementEndpoint();
        debtsettlementGroup.MapDeleteDebtSettlementEndpoint();
        // Documents endpoints
        RouteGroupBuilder documentGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/documents")
            .WithTags("Documents")
            .WithApiVersionSet(apiVersionSet);

        documentGroup.MapCreateDocumentEndpoint();
        documentGroup.MapGetDocumentsEndpoint();
        documentGroup.MapGetDocumentEndpoint();
        documentGroup.MapUpdateDocumentEndpoint();
        documentGroup.MapDeleteDocumentEndpoint();
        // FeeCharges endpoints
        RouteGroupBuilder feechargeGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/feecharges")
            .WithTags("FeeCharges")
            .WithApiVersionSet(apiVersionSet);

        feechargeGroup.MapCreateFeeChargeEndpoint();
        feechargeGroup.MapGetFeeChargesEndpoint();
        feechargeGroup.MapGetFeeChargeEndpoint();
        feechargeGroup.MapUpdateFeeChargeEndpoint();
        feechargeGroup.MapDeleteFeeChargeEndpoint();
        // FeeDefinitions endpoints
        RouteGroupBuilder feedefinitionGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/feedefinitions")
            .WithTags("FeeDefinitions")
            .WithApiVersionSet(apiVersionSet);

        feedefinitionGroup.MapCreateFeeDefinitionEndpoint();
        feedefinitionGroup.MapGetFeeDefinitionsEndpoint();
        feedefinitionGroup.MapGetFeeDefinitionEndpoint();
        feedefinitionGroup.MapUpdateFeeDefinitionEndpoint();
        feedefinitionGroup.MapDeleteFeeDefinitionEndpoint();
        // FeePayments endpoints
        RouteGroupBuilder feepaymentGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/feepayments")
            .WithTags("FeePayments")
            .WithApiVersionSet(apiVersionSet);

        feepaymentGroup.MapCreateFeePaymentEndpoint();
        feepaymentGroup.MapGetFeePaymentsEndpoint();
        feepaymentGroup.MapGetFeePaymentEndpoint();
        feepaymentGroup.MapUpdateFeePaymentEndpoint();
        feepaymentGroup.MapDeleteFeePaymentEndpoint();
        // FeeWaivers endpoints
        RouteGroupBuilder feewaiverGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/feewaivers")
            .WithTags("FeeWaivers")
            .WithApiVersionSet(apiVersionSet);

        feewaiverGroup.MapCreateFeeWaiverEndpoint();
        feewaiverGroup.MapGetFeeWaiversEndpoint();
        feewaiverGroup.MapGetFeeWaiverEndpoint();
        feewaiverGroup.MapUpdateFeeWaiverEndpoint();
        feewaiverGroup.MapDeleteFeeWaiverEndpoint();
        // FixedDeposits endpoints
        RouteGroupBuilder fixeddepositGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/fixeddeposits")
            .WithTags("FixedDeposits")
            .WithApiVersionSet(apiVersionSet);

        fixeddepositGroup.MapCreateFixedDepositEndpoint();
        fixeddepositGroup.MapGetFixedDepositsEndpoint();
        fixeddepositGroup.MapGetFixedDepositEndpoint();
        fixeddepositGroup.MapUpdateFixedDepositEndpoint();
        fixeddepositGroup.MapDeleteFixedDepositEndpoint();
        // GroupMemberships endpoints
        RouteGroupBuilder groupmembershipGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/groupmemberships")
            .WithTags("GroupMemberships")
            .WithApiVersionSet(apiVersionSet);

        groupmembershipGroup.MapCreateGroupMembershipEndpoint();
        groupmembershipGroup.MapGetGroupMembershipsEndpoint();
        groupmembershipGroup.MapGetGroupMembershipEndpoint();
        groupmembershipGroup.MapUpdateGroupMembershipEndpoint();
        groupmembershipGroup.MapDeleteGroupMembershipEndpoint();
        // InsuranceClaims endpoints
        RouteGroupBuilder insuranceclaimGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/insuranceclaims")
            .WithTags("InsuranceClaims")
            .WithApiVersionSet(apiVersionSet);

        insuranceclaimGroup.MapCreateInsuranceClaimEndpoint();
        insuranceclaimGroup.MapGetInsuranceClaimsEndpoint();
        insuranceclaimGroup.MapGetInsuranceClaimEndpoint();
        insuranceclaimGroup.MapUpdateInsuranceClaimEndpoint();
        insuranceclaimGroup.MapDeleteInsuranceClaimEndpoint();
        // InsurancePolicys endpoints
        RouteGroupBuilder insurancepolicyGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/insurancepolicys")
            .WithTags("InsurancePolicys")
            .WithApiVersionSet(apiVersionSet);

        insurancepolicyGroup.MapCreateInsurancePolicyEndpoint();
        insurancepolicyGroup.MapGetInsurancePolicysEndpoint();
        insurancepolicyGroup.MapGetInsurancePolicyEndpoint();
        insurancepolicyGroup.MapUpdateInsurancePolicyEndpoint();
        insurancepolicyGroup.MapDeleteInsurancePolicyEndpoint();
        // InsuranceProducts endpoints
        RouteGroupBuilder insuranceproductGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/insuranceproducts")
            .WithTags("InsuranceProducts")
            .WithApiVersionSet(apiVersionSet);

        insuranceproductGroup.MapCreateInsuranceProductEndpoint();
        insuranceproductGroup.MapGetInsuranceProductsEndpoint();
        insuranceproductGroup.MapGetInsuranceProductEndpoint();
        insuranceproductGroup.MapUpdateInsuranceProductEndpoint();
        insuranceproductGroup.MapDeleteInsuranceProductEndpoint();
        // InterestRateChanges endpoints
        RouteGroupBuilder interestratechangeGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/interestratechanges")
            .WithTags("InterestRateChanges")
            .WithApiVersionSet(apiVersionSet);

        interestratechangeGroup.MapCreateInterestRateChangeEndpoint();
        interestratechangeGroup.MapGetInterestRateChangesEndpoint();
        interestratechangeGroup.MapGetInterestRateChangeEndpoint();
        interestratechangeGroup.MapUpdateInterestRateChangeEndpoint();
        interestratechangeGroup.MapDeleteInterestRateChangeEndpoint();
        // InvestmentAccounts endpoints
        RouteGroupBuilder investmentaccountGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/investmentaccounts")
            .WithTags("InvestmentAccounts")
            .WithApiVersionSet(apiVersionSet);

        investmentaccountGroup.MapCreateInvestmentAccountEndpoint();
        investmentaccountGroup.MapGetInvestmentAccountsEndpoint();
        investmentaccountGroup.MapGetInvestmentAccountEndpoint();
        investmentaccountGroup.MapUpdateInvestmentAccountEndpoint();
        investmentaccountGroup.MapDeleteInvestmentAccountEndpoint();
        // InvestmentProducts endpoints
        RouteGroupBuilder investmentproductGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/investmentproducts")
            .WithTags("InvestmentProducts")
            .WithApiVersionSet(apiVersionSet);

        investmentproductGroup.MapCreateInvestmentProductEndpoint();
        investmentproductGroup.MapGetInvestmentProductsEndpoint();
        investmentproductGroup.MapGetInvestmentProductEndpoint();
        investmentproductGroup.MapUpdateInvestmentProductEndpoint();
        investmentproductGroup.MapDeleteInvestmentProductEndpoint();
        // InvestmentTransactions endpoints
        RouteGroupBuilder investmenttransactionGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/investmenttransactions")
            .WithTags("InvestmentTransactions")
            .WithApiVersionSet(apiVersionSet);

        investmenttransactionGroup.MapCreateInvestmentTransactionEndpoint();
        investmenttransactionGroup.MapGetInvestmentTransactionsEndpoint();
        investmenttransactionGroup.MapGetInvestmentTransactionEndpoint();
        investmenttransactionGroup.MapUpdateInvestmentTransactionEndpoint();
        investmenttransactionGroup.MapDeleteInvestmentTransactionEndpoint();
        // KycDocuments endpoints
        RouteGroupBuilder kycdocumentGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/kycdocuments")
            .WithTags("KycDocuments")
            .WithApiVersionSet(apiVersionSet);

        kycdocumentGroup.MapCreateKycDocumentEndpoint();
        kycdocumentGroup.MapGetKycDocumentsEndpoint();
        kycdocumentGroup.MapGetKycDocumentEndpoint();
        kycdocumentGroup.MapUpdateKycDocumentEndpoint();
        kycdocumentGroup.MapDeleteKycDocumentEndpoint();
        // LegalActions endpoints
        RouteGroupBuilder legalactionGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/legalactions")
            .WithTags("LegalActions")
            .WithApiVersionSet(apiVersionSet);

        legalactionGroup.MapCreateLegalActionEndpoint();
        legalactionGroup.MapGetLegalActionsEndpoint();
        legalactionGroup.MapGetLegalActionEndpoint();
        legalactionGroup.MapUpdateLegalActionEndpoint();
        legalactionGroup.MapDeleteLegalActionEndpoint();
        // Loans endpoints
        RouteGroupBuilder loanGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/loans")
            .WithTags("Loans")
            .WithApiVersionSet(apiVersionSet);

        loanGroup.MapCreateLoanEndpoint();
        loanGroup.MapGetLoansEndpoint();
        loanGroup.MapGetLoanEndpoint();
        loanGroup.MapUpdateLoanEndpoint();
        loanGroup.MapDeleteLoanEndpoint();
        // LoanApplications endpoints
        RouteGroupBuilder loanapplicationGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/loanapplications")
            .WithTags("LoanApplications")
            .WithApiVersionSet(apiVersionSet);

        loanapplicationGroup.MapCreateLoanApplicationEndpoint();
        loanapplicationGroup.MapGetLoanApplicationsEndpoint();
        loanapplicationGroup.MapGetLoanApplicationEndpoint();
        loanapplicationGroup.MapUpdateLoanApplicationEndpoint();
        loanapplicationGroup.MapDeleteLoanApplicationEndpoint();
        // LoanCollaterals endpoints
        RouteGroupBuilder loancollateralGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/loancollaterals")
            .WithTags("LoanCollaterals")
            .WithApiVersionSet(apiVersionSet);

        loancollateralGroup.MapCreateLoanCollateralEndpoint();
        loancollateralGroup.MapGetLoanCollateralsEndpoint();
        loancollateralGroup.MapGetLoanCollateralEndpoint();
        loancollateralGroup.MapUpdateLoanCollateralEndpoint();
        loancollateralGroup.MapDeleteLoanCollateralEndpoint();
        // LoanDisbursementTranches endpoints
        RouteGroupBuilder loandisbursementtrancheGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/loandisbursementtranches")
            .WithTags("LoanDisbursementTranches")
            .WithApiVersionSet(apiVersionSet);

        loandisbursementtrancheGroup.MapCreateLoanDisbursementTrancheEndpoint();
        loandisbursementtrancheGroup.MapGetLoanDisbursementTranchesEndpoint();
        loandisbursementtrancheGroup.MapGetLoanDisbursementTrancheEndpoint();
        loandisbursementtrancheGroup.MapUpdateLoanDisbursementTrancheEndpoint();
        loandisbursementtrancheGroup.MapDeleteLoanDisbursementTrancheEndpoint();
        // LoanGuarantors endpoints
        RouteGroupBuilder loanguarantorGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/loanguarantors")
            .WithTags("LoanGuarantors")
            .WithApiVersionSet(apiVersionSet);

        loanguarantorGroup.MapCreateLoanGuarantorEndpoint();
        loanguarantorGroup.MapGetLoanGuarantorsEndpoint();
        loanguarantorGroup.MapGetLoanGuarantorEndpoint();
        loanguarantorGroup.MapUpdateLoanGuarantorEndpoint();
        loanguarantorGroup.MapDeleteLoanGuarantorEndpoint();
        // LoanOfficerAssignments endpoints
        RouteGroupBuilder loanofficerassignmentGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/loanofficerassignments")
            .WithTags("LoanOfficerAssignments")
            .WithApiVersionSet(apiVersionSet);

        loanofficerassignmentGroup.MapCreateLoanOfficerAssignmentEndpoint();
        loanofficerassignmentGroup.MapGetLoanOfficerAssignmentsEndpoint();
        loanofficerassignmentGroup.MapGetLoanOfficerAssignmentEndpoint();
        loanofficerassignmentGroup.MapUpdateLoanOfficerAssignmentEndpoint();
        loanofficerassignmentGroup.MapDeleteLoanOfficerAssignmentEndpoint();
        // LoanOfficerTargets endpoints
        RouteGroupBuilder loanofficertargetGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/loanofficertargets")
            .WithTags("LoanOfficerTargets")
            .WithApiVersionSet(apiVersionSet);

        loanofficertargetGroup.MapCreateLoanOfficerTargetEndpoint();
        loanofficertargetGroup.MapGetLoanOfficerTargetsEndpoint();
        loanofficertargetGroup.MapGetLoanOfficerTargetEndpoint();
        loanofficertargetGroup.MapUpdateLoanOfficerTargetEndpoint();
        loanofficertargetGroup.MapDeleteLoanOfficerTargetEndpoint();
        // LoanProducts endpoints
        RouteGroupBuilder loanproductGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/loanproducts")
            .WithTags("LoanProducts")
            .WithApiVersionSet(apiVersionSet);

        loanproductGroup.MapCreateLoanProductEndpoint();
        loanproductGroup.MapGetLoanProductsEndpoint();
        loanproductGroup.MapGetLoanProductEndpoint();
        loanproductGroup.MapUpdateLoanProductEndpoint();
        loanproductGroup.MapDeleteLoanProductEndpoint();
        // LoanRepayments endpoints
        RouteGroupBuilder loanrepaymentGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/loanrepayments")
            .WithTags("LoanRepayments")
            .WithApiVersionSet(apiVersionSet);

        loanrepaymentGroup.MapCreateLoanRepaymentEndpoint();
        loanrepaymentGroup.MapGetLoanRepaymentsEndpoint();
        loanrepaymentGroup.MapGetLoanRepaymentEndpoint();
        loanrepaymentGroup.MapUpdateLoanRepaymentEndpoint();
        loanrepaymentGroup.MapDeleteLoanRepaymentEndpoint();
        // LoanRestructures endpoints
        RouteGroupBuilder loanrestructureGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/loanrestructures")
            .WithTags("LoanRestructures")
            .WithApiVersionSet(apiVersionSet);

        loanrestructureGroup.MapCreateLoanRestructureEndpoint();
        loanrestructureGroup.MapGetLoanRestructuresEndpoint();
        loanrestructureGroup.MapGetLoanRestructureEndpoint();
        loanrestructureGroup.MapUpdateLoanRestructureEndpoint();
        loanrestructureGroup.MapDeleteLoanRestructureEndpoint();
        // LoanSchedules endpoints
        RouteGroupBuilder loanscheduleGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/loanschedules")
            .WithTags("LoanSchedules")
            .WithApiVersionSet(apiVersionSet);

        loanscheduleGroup.MapCreateLoanScheduleEndpoint();
        loanscheduleGroup.MapGetLoanSchedulesEndpoint();
        loanscheduleGroup.MapGetLoanScheduleEndpoint();
        loanscheduleGroup.MapUpdateLoanScheduleEndpoint();
        loanscheduleGroup.MapDeleteLoanScheduleEndpoint();
        // LoanWriteOffs endpoints
        RouteGroupBuilder loanwriteoffGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/loanwriteoffs")
            .WithTags("LoanWriteOffs")
            .WithApiVersionSet(apiVersionSet);

        loanwriteoffGroup.MapCreateLoanWriteOffEndpoint();
        loanwriteoffGroup.MapGetLoanWriteOffsEndpoint();
        loanwriteoffGroup.MapGetLoanWriteOffEndpoint();
        loanwriteoffGroup.MapUpdateLoanWriteOffEndpoint();
        loanwriteoffGroup.MapDeleteLoanWriteOffEndpoint();
        // MarketingCampaigns endpoints
        RouteGroupBuilder marketingcampaignGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/marketingcampaigns")
            .WithTags("MarketingCampaigns")
            .WithApiVersionSet(apiVersionSet);

        marketingcampaignGroup.MapCreateMarketingCampaignEndpoint();
        marketingcampaignGroup.MapGetMarketingCampaignsEndpoint();
        marketingcampaignGroup.MapGetMarketingCampaignEndpoint();
        marketingcampaignGroup.MapUpdateMarketingCampaignEndpoint();
        marketingcampaignGroup.MapDeleteMarketingCampaignEndpoint();
        // MemberGroups endpoints
        RouteGroupBuilder membergroupGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/membergroups")
            .WithTags("MemberGroups")
            .WithApiVersionSet(apiVersionSet);

        membergroupGroup.MapCreateMemberGroupEndpoint();
        membergroupGroup.MapGetMemberGroupsEndpoint();
        membergroupGroup.MapGetMemberGroupEndpoint();
        membergroupGroup.MapUpdateMemberGroupEndpoint();
        membergroupGroup.MapDeleteMemberGroupEndpoint();
        // MfiConfigurations endpoints
        RouteGroupBuilder mficonfigurationGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/mficonfigurations")
            .WithTags("MfiConfigurations")
            .WithApiVersionSet(apiVersionSet);

        mficonfigurationGroup.MapCreateMfiConfigurationEndpoint();
        mficonfigurationGroup.MapGetMfiConfigurationsEndpoint();
        mficonfigurationGroup.MapGetMfiConfigurationEndpoint();
        mficonfigurationGroup.MapUpdateMfiConfigurationEndpoint();
        mficonfigurationGroup.MapDeleteMfiConfigurationEndpoint();
        // MobileTransactions endpoints
        RouteGroupBuilder mobiletransactionGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/mobiletransactions")
            .WithTags("MobileTransactions")
            .WithApiVersionSet(apiVersionSet);

        mobiletransactionGroup.MapCreateMobileTransactionEndpoint();
        mobiletransactionGroup.MapGetMobileTransactionsEndpoint();
        mobiletransactionGroup.MapGetMobileTransactionEndpoint();
        mobiletransactionGroup.MapUpdateMobileTransactionEndpoint();
        mobiletransactionGroup.MapDeleteMobileTransactionEndpoint();
        // MobileWallets endpoints
        RouteGroupBuilder mobilewalletGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/mobilewallets")
            .WithTags("MobileWallets")
            .WithApiVersionSet(apiVersionSet);

        mobilewalletGroup.MapCreateMobileWalletEndpoint();
        mobilewalletGroup.MapGetMobileWalletsEndpoint();
        mobilewalletGroup.MapGetMobileWalletEndpoint();
        mobilewalletGroup.MapUpdateMobileWalletEndpoint();
        mobilewalletGroup.MapDeleteMobileWalletEndpoint();
        // PaymentGateways endpoints
        RouteGroupBuilder paymentgatewayGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/paymentgateways")
            .WithTags("PaymentGateways")
            .WithApiVersionSet(apiVersionSet);

        paymentgatewayGroup.MapCreatePaymentGatewayEndpoint();
        paymentgatewayGroup.MapGetPaymentGatewaysEndpoint();
        paymentgatewayGroup.MapGetPaymentGatewayEndpoint();
        paymentgatewayGroup.MapUpdatePaymentGatewayEndpoint();
        paymentgatewayGroup.MapDeletePaymentGatewayEndpoint();
        // PromiseToPays endpoints
        RouteGroupBuilder promisetopayGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/promisetopays")
            .WithTags("PromiseToPays")
            .WithApiVersionSet(apiVersionSet);

        promisetopayGroup.MapCreatePromiseToPayEndpoint();
        promisetopayGroup.MapGetPromiseToPaysEndpoint();
        promisetopayGroup.MapGetPromiseToPayEndpoint();
        promisetopayGroup.MapUpdatePromiseToPayEndpoint();
        promisetopayGroup.MapDeletePromiseToPayEndpoint();
        // QrPayments endpoints
        RouteGroupBuilder qrpaymentGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/qrpayments")
            .WithTags("QrPayments")
            .WithApiVersionSet(apiVersionSet);

        qrpaymentGroup.MapCreateQrPaymentEndpoint();
        qrpaymentGroup.MapGetQrPaymentsEndpoint();
        qrpaymentGroup.MapGetQrPaymentEndpoint();
        qrpaymentGroup.MapUpdateQrPaymentEndpoint();
        qrpaymentGroup.MapDeleteQrPaymentEndpoint();
        // ReportDefinitions endpoints
        RouteGroupBuilder reportdefinitionGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/reportdefinitions")
            .WithTags("ReportDefinitions")
            .WithApiVersionSet(apiVersionSet);

        reportdefinitionGroup.MapCreateReportDefinitionEndpoint();
        reportdefinitionGroup.MapGetReportDefinitionsEndpoint();
        reportdefinitionGroup.MapGetReportDefinitionEndpoint();
        reportdefinitionGroup.MapUpdateReportDefinitionEndpoint();
        reportdefinitionGroup.MapDeleteReportDefinitionEndpoint();
        // ReportGenerations endpoints
        RouteGroupBuilder reportgenerationGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/reportgenerations")
            .WithTags("ReportGenerations")
            .WithApiVersionSet(apiVersionSet);

        reportgenerationGroup.MapCreateReportGenerationEndpoint();
        reportgenerationGroup.MapGetReportGenerationsEndpoint();
        reportgenerationGroup.MapGetReportGenerationEndpoint();
        reportgenerationGroup.MapUpdateReportGenerationEndpoint();
        reportgenerationGroup.MapDeleteReportGenerationEndpoint();
        // RiskAlerts endpoints
        RouteGroupBuilder riskalertGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/riskalerts")
            .WithTags("RiskAlerts")
            .WithApiVersionSet(apiVersionSet);

        riskalertGroup.MapCreateRiskAlertEndpoint();
        riskalertGroup.MapGetRiskAlertsEndpoint();
        riskalertGroup.MapGetRiskAlertEndpoint();
        riskalertGroup.MapUpdateRiskAlertEndpoint();
        riskalertGroup.MapDeleteRiskAlertEndpoint();
        // RiskCategorys endpoints
        RouteGroupBuilder riskcategoryGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/riskcategorys")
            .WithTags("RiskCategorys")
            .WithApiVersionSet(apiVersionSet);

        riskcategoryGroup.MapCreateRiskCategoryEndpoint();
        riskcategoryGroup.MapGetRiskCategorysEndpoint();
        riskcategoryGroup.MapGetRiskCategoryEndpoint();
        riskcategoryGroup.MapUpdateRiskCategoryEndpoint();
        riskcategoryGroup.MapDeleteRiskCategoryEndpoint();
        // RiskIndicators endpoints
        RouteGroupBuilder riskindicatorGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/riskindicators")
            .WithTags("RiskIndicators")
            .WithApiVersionSet(apiVersionSet);

        riskindicatorGroup.MapCreateRiskIndicatorEndpoint();
        riskindicatorGroup.MapGetRiskIndicatorsEndpoint();
        riskindicatorGroup.MapGetRiskIndicatorEndpoint();
        riskindicatorGroup.MapUpdateRiskIndicatorEndpoint();
        riskindicatorGroup.MapDeleteRiskIndicatorEndpoint();
        // SavingsAccounts endpoints
        RouteGroupBuilder savingsaccountGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/savingsaccounts")
            .WithTags("SavingsAccounts")
            .WithApiVersionSet(apiVersionSet);

        savingsaccountGroup.MapCreateSavingsAccountEndpoint();
        savingsaccountGroup.MapGetSavingsAccountsEndpoint();
        savingsaccountGroup.MapGetSavingsAccountEndpoint();
        savingsaccountGroup.MapUpdateSavingsAccountEndpoint();
        savingsaccountGroup.MapDeleteSavingsAccountEndpoint();
        // SavingsProducts endpoints
        RouteGroupBuilder savingsproductGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/savingsproducts")
            .WithTags("SavingsProducts")
            .WithApiVersionSet(apiVersionSet);

        savingsproductGroup.MapCreateSavingsProductEndpoint();
        savingsproductGroup.MapGetSavingsProductsEndpoint();
        savingsproductGroup.MapGetSavingsProductEndpoint();
        savingsproductGroup.MapUpdateSavingsProductEndpoint();
        savingsproductGroup.MapDeleteSavingsProductEndpoint();
        // SavingsTransactions endpoints
        RouteGroupBuilder savingstransactionGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/savingstransactions")
            .WithTags("SavingsTransactions")
            .WithApiVersionSet(apiVersionSet);

        savingstransactionGroup.MapCreateSavingsTransactionEndpoint();
        savingstransactionGroup.MapGetSavingsTransactionsEndpoint();
        savingstransactionGroup.MapGetSavingsTransactionEndpoint();
        savingstransactionGroup.MapUpdateSavingsTransactionEndpoint();
        savingstransactionGroup.MapDeleteSavingsTransactionEndpoint();
        // ShareAccounts endpoints
        RouteGroupBuilder shareaccountGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/shareaccounts")
            .WithTags("ShareAccounts")
            .WithApiVersionSet(apiVersionSet);

        shareaccountGroup.MapCreateShareAccountEndpoint();
        shareaccountGroup.MapGetShareAccountsEndpoint();
        shareaccountGroup.MapGetShareAccountEndpoint();
        shareaccountGroup.MapUpdateShareAccountEndpoint();
        shareaccountGroup.MapDeleteShareAccountEndpoint();
        // ShareProducts endpoints
        RouteGroupBuilder shareproductGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/shareproducts")
            .WithTags("ShareProducts")
            .WithApiVersionSet(apiVersionSet);

        shareproductGroup.MapCreateShareProductEndpoint();
        shareproductGroup.MapGetShareProductsEndpoint();
        shareproductGroup.MapGetShareProductEndpoint();
        shareproductGroup.MapUpdateShareProductEndpoint();
        shareproductGroup.MapDeleteShareProductEndpoint();
        // ShareTransactions endpoints
        RouteGroupBuilder sharetransactionGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/sharetransactions")
            .WithTags("ShareTransactions")
            .WithApiVersionSet(apiVersionSet);

        sharetransactionGroup.MapCreateShareTransactionEndpoint();
        sharetransactionGroup.MapGetShareTransactionsEndpoint();
        sharetransactionGroup.MapGetShareTransactionEndpoint();
        sharetransactionGroup.MapUpdateShareTransactionEndpoint();
        sharetransactionGroup.MapDeleteShareTransactionEndpoint();
        // Staffs endpoints
        RouteGroupBuilder staffGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/staffs")
            .WithTags("Staffs")
            .WithApiVersionSet(apiVersionSet);

        staffGroup.MapCreateStaffEndpoint();
        staffGroup.MapGetStaffsEndpoint();
        staffGroup.MapGetStaffEndpoint();
        staffGroup.MapUpdateStaffEndpoint();
        staffGroup.MapDeleteStaffEndpoint();
        // StaffTrainings endpoints
        RouteGroupBuilder stafftrainingGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/stafftrainings")
            .WithTags("StaffTrainings")
            .WithApiVersionSet(apiVersionSet);

        stafftrainingGroup.MapCreateStaffTrainingEndpoint();
        stafftrainingGroup.MapGetStaffTrainingsEndpoint();
        stafftrainingGroup.MapGetStaffTrainingEndpoint();
        stafftrainingGroup.MapUpdateStaffTrainingEndpoint();
        stafftrainingGroup.MapDeleteStaffTrainingEndpoint();
        // TellerSessions endpoints
        RouteGroupBuilder tellersessionGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/tellersessions")
            .WithTags("TellerSessions")
            .WithApiVersionSet(apiVersionSet);

        tellersessionGroup.MapCreateTellerSessionEndpoint();
        tellersessionGroup.MapGetTellerSessionsEndpoint();
        tellersessionGroup.MapGetTellerSessionEndpoint();
        tellersessionGroup.MapUpdateTellerSessionEndpoint();
        tellersessionGroup.MapDeleteTellerSessionEndpoint();
        // UssdSessions endpoints
        RouteGroupBuilder ussdsessionGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/ussdsessions")
            .WithTags("UssdSessions")
            .WithApiVersionSet(apiVersionSet);

        ussdsessionGroup.MapCreateUssdSessionEndpoint();
        ussdsessionGroup.MapGetUssdSessionsEndpoint();
        ussdsessionGroup.MapGetUssdSessionEndpoint();
        ussdsessionGroup.MapUpdateUssdSessionEndpoint();
        ussdsessionGroup.MapDeleteUssdSessionEndpoint();
    }
}
