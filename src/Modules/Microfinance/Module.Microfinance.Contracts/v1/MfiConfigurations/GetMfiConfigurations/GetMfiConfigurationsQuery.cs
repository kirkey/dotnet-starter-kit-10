using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MfiConfigurations.GetMfiConfigurations;

public sealed record GetMfiConfigurationsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<MfiConfigurationsPagedResponse>;
