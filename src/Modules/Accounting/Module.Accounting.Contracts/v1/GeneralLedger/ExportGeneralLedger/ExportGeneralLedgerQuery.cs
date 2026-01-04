namespace FSH.Module.Accounting.Contracts.v1.GeneralLedger;

public sealed record ExportGeneralLedgerQuery(Guid Id, string Format = "pdf") : IQuery<ExportGeneralLedgerResult>;

public sealed record ExportGeneralLedgerResult(byte[] Data, string ContentType, string FileName);