using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CustomerSurveys.DeleteCustomerSurvey;

public sealed record DeleteCustomerSurveyCommand(Guid Id) : ICommand;
