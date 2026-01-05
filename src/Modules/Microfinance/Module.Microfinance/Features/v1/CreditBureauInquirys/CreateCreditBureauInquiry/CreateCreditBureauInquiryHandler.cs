using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.CreditBureauInquirys.CreateCreditBureauInquiry;

namespace FSH.Module.Microfinance.Features.v1.CreditBureauInquirys.CreateCreditBureauInquiry;

public class CreateCreditBureauInquiryHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateCreditBureauInquiryCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCreditBureauInquiryCommand command, CancellationToken ct)
    {
        var entity = CreditBureauInquiry.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.CreditBureauInquirys.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
