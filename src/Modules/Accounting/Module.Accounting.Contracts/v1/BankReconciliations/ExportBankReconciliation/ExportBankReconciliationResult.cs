namespace FSH.Module.Accounting.Contracts.v1.BankReconciliations.ExportBankReconciliation;

public sealed record ExportBankReconciliationResult(byte[] Data, string ContentType, string FileName);