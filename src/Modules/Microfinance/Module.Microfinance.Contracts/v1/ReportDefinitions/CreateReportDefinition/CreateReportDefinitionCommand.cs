using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ReportDefinitions.CreateReportDefinition;

public sealed record CreateReportDefinitionCommand(string Name) : ICommand<Guid>;
