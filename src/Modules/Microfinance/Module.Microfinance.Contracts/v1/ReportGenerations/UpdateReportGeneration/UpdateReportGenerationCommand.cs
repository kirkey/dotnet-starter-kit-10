using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.ReportGenerations.UpdateReportGeneration;

public sealed record UpdateReportGenerationCommand(Guid Id, string Name) : ICommand<Guid>;
