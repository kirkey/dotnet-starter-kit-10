using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.TaxCodes.CreateTaxCode;

public sealed record CreateTaxCodeCommand(string Name, string? Description = null) : ICommand<Guid>;