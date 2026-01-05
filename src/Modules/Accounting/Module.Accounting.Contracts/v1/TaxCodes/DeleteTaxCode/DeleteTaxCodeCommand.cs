using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.TaxCodes.DeleteTaxCode;

public sealed record DeleteTaxCodeCommand(Guid Id) : ICommand;