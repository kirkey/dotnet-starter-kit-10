using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CustomerSurveys.DeleteCustomerSurvey;

public record DeleteCustomerSurveyCommand(Guid Id) : ICommand;

public class DeleteCustomerSurveyHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteCustomerSurveyCommand>
{
    public async ValueTask<Unit> Handle(DeleteCustomerSurveyCommand command, CancellationToken ct)
    {
        var entity = await context.CustomerSurveys.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CustomerSurvey not found");
        
        context.CustomerSurveys.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
