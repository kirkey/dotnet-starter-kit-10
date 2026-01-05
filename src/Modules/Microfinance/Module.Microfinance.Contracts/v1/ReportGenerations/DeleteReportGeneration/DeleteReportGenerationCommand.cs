using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ReportGenerations.DeleteReportGeneration;

public sealed record DeleteReportGenerationCommand(Guid Id) : ICommand;
