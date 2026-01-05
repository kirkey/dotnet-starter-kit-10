using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.MarketingCampaigns.DeleteMarketingCampaign;

namespace FSH.Module.Microfinance.Features.v1.MarketingCampaigns.DeleteMarketingCampaign;

public class DeleteMarketingCampaignHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteMarketingCampaignCommand>
{
    public async ValueTask<Unit> Handle(DeleteMarketingCampaignCommand command, CancellationToken ct)
    {
        var entity = await context.MarketingCampaigns.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("MarketingCampaign not found");
        
        context.MarketingCampaigns.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
