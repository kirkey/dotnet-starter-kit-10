using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.TaxCodes.GetTaxCode;

public sealed record GetTaxCodeQuery(Guid Id) : IQuery<TaxCodeDto>;