using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.InvoiceLineItems.CreateInvoiceLineItem;

namespace FSH.Module.Accounting.Features.v1.InvoiceLineItems.CreateInvoiceLineItem;

public class CreateInvoiceLineItemHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateInvoiceLineItemCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateInvoiceLineItemCommand command, CancellationToken ct)
    {
        var entity = InvoiceLineItem.Create(
            command.InvoiceId,
            command.LineNumber,
            command.ItemDescription,
            command.AccountId,
            command.AccountCode,
            command.Quantity,
            command.UnitPrice,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.ItemCode,
            command.UnitOfMeasure,
            command.DiscountPercent,
            command.TaxCode,
            command.TaxRate,
            command.Notes);
        
        context.InvoiceLineItems.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
