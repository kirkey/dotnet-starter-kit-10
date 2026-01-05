using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ReportGenerations.CreateReportGeneration;

public sealed record CreateReportGenerationCommand(string Name) : ICommand<Guid>;
