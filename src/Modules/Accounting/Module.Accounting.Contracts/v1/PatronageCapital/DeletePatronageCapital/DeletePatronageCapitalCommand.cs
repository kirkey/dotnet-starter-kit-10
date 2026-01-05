using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PatronageCapital.DeletePatronageCapital;

public record DeletePatronageCapitalCommand(Guid Id) : ICommand;