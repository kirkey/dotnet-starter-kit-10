using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ReportGenerations.GetReportGeneration;

public sealed record GetReportGenerationQuery(Guid Id) : IQuery<ReportGenerationDto>;
