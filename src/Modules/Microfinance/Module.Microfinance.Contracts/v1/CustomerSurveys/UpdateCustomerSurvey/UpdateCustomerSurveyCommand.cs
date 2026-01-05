using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CustomerSurveys.UpdateCustomerSurvey;

public sealed record UpdateCustomerSurveyCommand(Guid Id, string Name) : ICommand<Guid>;
