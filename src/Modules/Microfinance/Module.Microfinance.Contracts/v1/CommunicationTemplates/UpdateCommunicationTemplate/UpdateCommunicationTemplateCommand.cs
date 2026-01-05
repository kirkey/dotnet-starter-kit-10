using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CommunicationTemplates.UpdateCommunicationTemplate;

public sealed record UpdateCommunicationTemplateCommand(Guid Id, string Name) : ICommand<Guid>;
