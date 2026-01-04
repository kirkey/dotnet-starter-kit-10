using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CreditBureauInquirys.DeleteCreditBureauInquiry;

public record DeleteCreditBureauInquiryCommand(Guid Id) : ICommand;

public class DeleteCreditBureauInquiryHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteCreditBureauInquiryCommand>
{
    public async ValueTask<Unit> Handle(DeleteCreditBureauInquiryCommand command, CancellationToken ct)
    {
        var entity = await context.CreditBureauInquirys.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CreditBureauInquiry not found");
        
        context.CreditBureauInquirys.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
