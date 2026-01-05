using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PatronageCapital.GetPatronageCapital;

public record GetPatronageCapitalQuery(Guid Id) : IQuery<PatronageCapitalDto>;