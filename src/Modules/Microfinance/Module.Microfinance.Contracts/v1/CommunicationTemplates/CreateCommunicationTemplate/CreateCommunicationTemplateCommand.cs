using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CommunicationTemplates.CreateCommunicationTemplate;

public sealed record CreateCommunicationTemplateCommand(string Name) : ICommand<Guid>;
