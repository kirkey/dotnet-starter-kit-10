using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ReportDefinitions.UpdateReportDefinition;

public sealed record UpdateReportDefinitionCommand(Guid Id, string Name) : ICommand<Guid>;
