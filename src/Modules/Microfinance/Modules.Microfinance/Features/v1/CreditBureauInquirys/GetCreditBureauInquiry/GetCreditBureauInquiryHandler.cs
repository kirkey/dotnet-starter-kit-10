using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.CreditBureauInquirys;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CreditBureauInquirys.GetCreditBureauInquiry;

public record GetCreditBureauInquiryQuery(Guid Id) : IQuery<CreditBureauInquiryDto>;

public class GetCreditBureauInquiryHandler(MicrofinanceDbContext context) : IQueryHandler<GetCreditBureauInquiryQuery, CreditBureauInquiryDto>
{
    public async ValueTask<CreditBureauInquiryDto> Handle(GetCreditBureauInquiryQuery query, CancellationToken ct)
    {
        var entity = await context.CreditBureauInquirys
            .Where(x => x.Id == query.Id)
            .Select(x => new CreditBureauInquiryDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("CreditBureauInquiry not found");
    }
}
