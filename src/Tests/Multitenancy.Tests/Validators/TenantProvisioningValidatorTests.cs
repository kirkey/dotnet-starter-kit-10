using FSH.Modules.Multitenancy.Features.v1.TenantProvisioning.RetryTenantProvisioning;
using FSH.Modules.Multitenancy.Features.v1.ResetTenantTheme;
using FSH.Modules.Multitenancy.Contracts.v1.TenantProvisioning;
using FSH.Modules.Multitenancy.Contracts.v1.ResetTenantTheme;
using Shouldly;
using Xunit;

namespace Multitenancy.Tests.Validators;

public sealed class TenantProvisioningValidatorTests
{
    [Fact]
    public void RetryTenantProvisioning_rejects_empty_tenant_id()
        => new RetryTenantProvisioningCommandValidator().Validate(new RetryTenantProvisioningCommand(string.Empty)).IsValid.ShouldBeFalse();

    [Fact]
    public void RetryTenantProvisioning_accepts_valid_command()
        => new RetryTenantProvisioningCommandValidator().Validate(new RetryTenantProvisioningCommand("tenant1")).IsValid.ShouldBeTrue();

    [Fact]
    public void ResetTenantTheme_accepts_parameterless_command()
        => new ResetTenantThemeCommandValidator().Validate(new ResetTenantThemeCommand()).IsValid.ShouldBeTrue();
}
