using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.CustomerSurveys.CreateCustomerSurvey;

public record CreateCustomerSurveyCommand(string Name) : ICommand<Guid>;

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
