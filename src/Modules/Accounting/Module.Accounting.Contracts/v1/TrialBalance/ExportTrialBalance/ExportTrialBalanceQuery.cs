using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.TrialBalance.ExportTrialBalance;

public sealed record ExportTrialBalanceQuery(Guid Id, string Format = "pdf") : IQuery<ExportTrialBalanceResult>;

public sealed record ExportTrialBalanceResult(byte[] Data, string ContentType, string FileName);