namespace FSH.Module.Accounting.Contracts.v1.BankReconciliations;

public sealed record ExportBankReconciliationQuery(Guid Id, string Format = "pdf") : IQuery<ExportBankReconciliationResult>;

public sealed record ExportBankReconciliationResult(byte[] Data, string ContentType, string FileName);