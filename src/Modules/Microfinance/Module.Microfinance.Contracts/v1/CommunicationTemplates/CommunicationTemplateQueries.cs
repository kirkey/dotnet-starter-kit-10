namespace FSH.Module.Microfinance.Contracts.v1.CommunicationTemplates;

public record GetCommunicationTemplateQuery(Guid Id);
public record GetCommunicationTemplatesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record CommunicationTemplatesPagedResponse(List<CommunicationTemplateSummaryDto> Items, int TotalCount, int Page, int PageSize);
