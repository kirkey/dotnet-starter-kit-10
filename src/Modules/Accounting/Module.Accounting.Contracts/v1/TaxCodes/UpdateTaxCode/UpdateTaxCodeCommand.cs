using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.TaxCodes.UpdateTaxCode;

public sealed record UpdateTaxCodeCommand(Guid Id, string Name, string? Description = null) : ICommand<Guid>;