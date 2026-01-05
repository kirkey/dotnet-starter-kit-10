using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ReportDefinitions.DeleteReportDefinition;

public sealed record DeleteReportDefinitionCommand(Guid Id) : ICommand;
