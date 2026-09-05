using FSH.Modules.Billing.Features.v1.Invoices.VoidInvoice;
using FSH.Modules.Billing.Features.v1.Invoices.MarkInvoicePaid;
using FSH.Modules.Billing.Features.v1.Invoices.IssueInvoice;
using FSH.Modules.Billing.Contracts.v1.Invoices;
using Shouldly;
using Xunit;

namespace Billing.Tests.Validators;

public sealed class InvoiceCommandValidatorTests
{
    [Fact]
    public void VoidInvoice_rejects_empty_id()
        => new VoidInvoiceCommandValidator().Validate(new VoidInvoiceCommand(Guid.Empty)).IsValid.ShouldBeFalse();

    [Fact]
    public void VoidInvoice_rejects_overlong_reason()
        => new VoidInvoiceCommandValidator().Validate(new VoidInvoiceCommand(Guid.NewGuid(), new string('x', 513))).IsValid.ShouldBeFalse();

    [Fact]
    public void VoidInvoice_accepts_valid_command()
        => new VoidInvoiceCommandValidator().Validate(new VoidInvoiceCommand(Guid.NewGuid(), "duplicate")).IsValid.ShouldBeTrue();

    [Fact]
    public void MarkInvoicePaid_rejects_empty_id()
        => new MarkInvoicePaidCommandValidator().Validate(new MarkInvoicePaidCommand(Guid.Empty)).IsValid.ShouldBeFalse();

    [Fact]
    public void MarkInvoicePaid_accepts_valid_command()
        => new MarkInvoicePaidCommandValidator().Validate(new MarkInvoicePaidCommand(Guid.NewGuid())).IsValid.ShouldBeTrue();

    [Fact]
    public void IssueInvoice_rejects_empty_id()
        => new IssueInvoiceCommandValidator().Validate(new IssueInvoiceCommand(Guid.Empty)).IsValid.ShouldBeFalse();

    [Fact]
    public void IssueInvoice_accepts_valid_command()
        => new IssueInvoiceCommandValidator().Validate(new IssueInvoiceCommand(Guid.NewGuid())).IsValid.ShouldBeTrue();
}
