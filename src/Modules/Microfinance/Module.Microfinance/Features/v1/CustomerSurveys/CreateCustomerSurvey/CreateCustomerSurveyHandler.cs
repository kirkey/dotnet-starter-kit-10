using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.CustomerSurveys.CreateCustomerSurvey;

namespace FSH.Module.Microfinance.Features.v1.CustomerSurveys.CreateCustomerSurvey;

public class CreateCustomerSurveyHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateCustomerSurveyCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCustomerSurveyCommand command, CancellationToken ct)
    {
        var entity = CustomerSurvey.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.CustomerSurveys.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
