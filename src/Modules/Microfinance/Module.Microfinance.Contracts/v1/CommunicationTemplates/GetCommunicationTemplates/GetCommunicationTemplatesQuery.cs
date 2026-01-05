using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CommunicationTemplates.GetCommunicationTemplates;

public sealed record GetCommunicationTemplatesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CommunicationTemplatesPagedResponse>;
