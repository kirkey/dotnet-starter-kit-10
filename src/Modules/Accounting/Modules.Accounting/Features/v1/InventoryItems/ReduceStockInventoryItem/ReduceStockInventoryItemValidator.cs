using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.InventoryItems.ReduceStockInventoryItem;

public class ReduceStockInventoryItemValidator : AbstractValidator<ReduceStockInventoryItemCommand>
{
    public ReduceStockInventoryItemValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}
