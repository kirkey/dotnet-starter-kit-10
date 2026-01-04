using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.InventoryItems.AddStockInventoryItem;

namespace FSH.Module.Accounting.Features.v1.InventoryItems.AddStockInventoryItem;

public class AddStockInventoryItemValidator : AbstractValidator<AddStockInventoryItemCommand>
{
    public AddStockInventoryItemValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}
