using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.InventoryItems.ReduceStockInventoryItem;

namespace FSH.Module.Accounting.Features.v1.InventoryItems.ReduceStockInventoryItem;

public class ReduceStockInventoryItemValidator : AbstractValidator<ReduceStockInventoryItemCommand>
{
    public ReduceStockInventoryItemValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}
