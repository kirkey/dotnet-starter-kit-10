// TODO: Implement Approve operation for Invoice
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Invoices.ApproveInvoice;

public record ApproveInvoiceCommand(Guid Id) : ICommand;

public class ApproveInvoiceHandler(AccountingDbContext context) 
    : ICommandHandler<ApproveInvoiceCommand>
{
    public async ValueTask<Unit> Handle(ApproveInvoiceCommand command, CancellationToken ct)
    {
        // TODO: Implement Approve logic
        throw new NotImplementedException("Approve operation for Invoice needs to be implemented");
    }
}
