// TODO: Implement Recognize operation for DeferredRevenue
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.DeferredRevenue.RecognizeDeferredRevenue;

public record RecognizeDeferredRevenueCommand(Guid Id) : ICommand;

public class RecognizeDeferredRevenueHandler(AccountingDbContext context) 
    : ICommandHandler<RecognizeDeferredRevenueCommand>
{
    public async ValueTask<Unit> Handle(RecognizeDeferredRevenueCommand command, CancellationToken ct)
    {
        // TODO: Implement Recognize logic
        throw new NotImplementedException("Recognize operation for DeferredRevenue needs to be implemented");
    }
}
