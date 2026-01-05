using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PatronageCapital.CreatePatronageCapital;

public record CreatePatronageCapitalCommand(string Name, string? Description) : ICommand<Guid>;