using FSH.Modules.Billing.Features.v1.Invoices.GetMyInvoices;
using FSH.Modules.Billing.Features.v1.Invoices.GetInvoices;
using FSH.Modules.Billing.Contracts.v1.Invoices;
using Shouldly;
using Xunit;

namespace Billing.Tests.Validators;

public sealed class InvoiceQueryValidatorTests
{
    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 0)]
    [InlineData(1, 101)]
    public void GetMyInvoices_rejects_out_of_range_paging(int page, int size)
        => new GetMyInvoicesQueryValidator().Validate(new GetMyInvoicesQuery(PageNumber: page, PageSize: size)).IsValid.ShouldBeFalse();

    [Fact]
    public void GetMyInvoices_rejects_invalid_month()
        => new GetMyInvoicesQueryValidator().Validate(new GetMyInvoicesQuery(PeriodMonth: 13)).IsValid.ShouldBeFalse();

    [Fact]
    public void GetMyInvoices_accepts_valid_query()
        => new GetMyInvoicesQueryValidator().Validate(new GetMyInvoicesQuery(PeriodMonth: 6)).IsValid.ShouldBeTrue();

    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 0)]
    [InlineData(1, 101)]
    public void GetInvoices_rejects_out_of_range_paging(int page, int size)
        => new GetInvoicesQueryValidator().Validate(new GetInvoicesQuery(PageNumber: page, PageSize: size)).IsValid.ShouldBeFalse();

    [Fact]
    public void GetInvoices_rejects_invalid_month()
        => new GetInvoicesQueryValidator().Validate(new GetInvoicesQuery(PeriodMonth: 13)).IsValid.ShouldBeFalse();

    [Fact]
    public void GetInvoices_accepts_valid_query()
        => new GetInvoicesQueryValidator().Validate(new GetInvoicesQuery()).IsValid.ShouldBeTrue();
}
