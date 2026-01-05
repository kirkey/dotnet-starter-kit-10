using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CommunicationTemplates.GetCommunicationTemplate;

public sealed record GetCommunicationTemplateQuery(Guid Id) : IQuery<CommunicationTemplateDto>;
