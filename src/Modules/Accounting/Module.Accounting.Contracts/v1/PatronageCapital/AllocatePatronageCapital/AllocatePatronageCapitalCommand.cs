using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PatronageCapital.AllocatePatronageCapital;

public record AllocatePatronageCapitalCommand(Guid Id) : ICommand;