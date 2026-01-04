using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CustomerSurveys.UpdateCustomerSurvey;

public record UpdateCustomerSurveyCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateCustomerSurveyHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateCustomerSurveyCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateCustomerSurveyCommand command, CancellationToken ct)
    {
        var entity = await context.CustomerSurveys.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CustomerSurvey not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
