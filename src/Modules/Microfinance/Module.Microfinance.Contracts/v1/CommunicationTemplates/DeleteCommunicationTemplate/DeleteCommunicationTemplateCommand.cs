using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CommunicationTemplates.DeleteCommunicationTemplate;

public sealed record DeleteCommunicationTemplateCommand(Guid Id) : ICommand;
