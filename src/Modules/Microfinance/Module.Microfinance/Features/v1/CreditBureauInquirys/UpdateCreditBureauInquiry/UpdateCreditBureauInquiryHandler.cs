using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CreditBureauInquirys.UpdateCreditBureauInquiry;

public record UpdateCreditBureauInquiryCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateCreditBureauInquiryHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateCreditBureauInquiryCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateCreditBureauInquiryCommand command, CancellationToken ct)
    {
        var entity = await context.CreditBureauInquirys.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CreditBureauInquiry not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
