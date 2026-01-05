namespace FSH.Module.Accounting.Contracts.v1.TrialBalance;

public sealed record ExportTrialBalanceQuery(Guid Id, string Format = "pdf") : IQuery<ExportTrialBalanceResult>
{
    public override bool Equals(object obj)
    {
        return Equals(obj as ExportTrialBalanceQuery);
    }
}

public sealed record ExportTrialBalanceResult(byte[] Data, string ContentType, string FileName);