using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ReportDefinitions.GetReportDefinition;

public sealed record GetReportDefinitionQuery(Guid Id) : IQuery<ReportDefinitionDto>;
