using FSH.Modules.Identity.Features.v1.Sessions.GetTenantSessions;
using FSH.Modules.Identity.Contracts.v1.Sessions.GetTenantSessions;
using Shouldly;
using Xunit;

namespace Identity.Tests.Validators;

public sealed class GetTenantSessionsQueryValidatorTests
{
    [Theory]
    [InlineData(0, 50)]
    [InlineData(1, 0)]
    [InlineData(1, 201)]
    public void Rejects_out_of_range_paging(int page, int size)
        => new GetTenantSessionsQueryValidator().Validate(new GetTenantSessionsQuery() { PageNumber = page, PageSize = size }).IsValid.ShouldBeFalse();

    [Fact]
    public void Accepts_valid_query()
        => new GetTenantSessionsQueryValidator().Validate(new GetTenantSessionsQuery()).IsValid.ShouldBeTrue();
}
