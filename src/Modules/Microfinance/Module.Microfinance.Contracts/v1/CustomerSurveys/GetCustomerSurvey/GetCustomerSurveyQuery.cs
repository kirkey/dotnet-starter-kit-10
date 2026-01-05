using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CustomerSurveys.GetCustomerSurvey;

public sealed record GetCustomerSurveyQuery(Guid Id) : IQuery<CustomerSurveyDto>;
