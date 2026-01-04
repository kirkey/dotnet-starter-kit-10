using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.CustomerSurveys;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CustomerSurveys.GetCustomerSurvey;

public record GetCustomerSurveyQuery(Guid Id) : IQuery<CustomerSurveyDto>;

public class GetCustomerSurveyHandler(MicrofinanceDbContext context) : IQueryHandler<GetCustomerSurveyQuery, CustomerSurveyDto>
{
    public async ValueTask<CustomerSurveyDto> Handle(GetCustomerSurveyQuery query, CancellationToken ct)
    {
        var entity = await context.CustomerSurveys
            .Where(x => x.Id == query.Id)
            .Select(x => new CustomerSurveyDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("CustomerSurvey not found");
    }
}
