using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PatronageCapital.UpdatePatronageCapital;

public record UpdatePatronageCapitalCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;