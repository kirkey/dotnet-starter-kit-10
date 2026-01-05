using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CustomerSurveys.CreateCustomerSurvey;

public sealed record CreateCustomerSurveyCommand(string Name) : ICommand<Guid>;
