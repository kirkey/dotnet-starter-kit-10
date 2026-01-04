using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.MarketingCampaigns.UpdateMarketingCampaign;

public record UpdateMarketingCampaignCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateMarketingCampaignHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateMarketingCampaignCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateMarketingCampaignCommand command, CancellationToken ct)
    {
        var entity = await context.MarketingCampaigns.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("MarketingCampaign not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
