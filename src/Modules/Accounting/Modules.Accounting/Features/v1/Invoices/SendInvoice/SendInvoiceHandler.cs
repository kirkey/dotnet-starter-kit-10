// TODO: Implement Send operation for Invoice
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Invoices.SendInvoice;

public record SendInvoiceCommand(Guid Id) : ICommand;

public class SendInvoiceHandler(AccountingDbContext context) 
    : ICommandHandler<SendInvoiceCommand>
{
    public async ValueTask<Unit> Handle(SendInvoiceCommand command, CancellationToken ct)
    {
        // TODO: Implement Send logic
        throw new NotImplementedException("Send operation for Invoice needs to be implemented");
    }
}
