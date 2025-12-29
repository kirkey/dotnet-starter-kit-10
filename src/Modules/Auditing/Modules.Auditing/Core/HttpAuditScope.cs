using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Shared.Multitenancy;
using FSH.Modules.Auditing.Contracts;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Security.Claims;

namespace FSH.Modules.Auditing.Core;

public sealed class HttpAuditScope(
    IHttpContextAccessor httpContextAccessor,
    IMultiTenantContextAccessor<AppTenantInfo> tenantAccessor)
    : IAuditScope
{
    public string? TenantId =>
        tenantAccessor.MultiTenantContext?.TenantInfo?.Id
        ?? httpContextAccessor.HttpContext?.User?.FindFirstValue(MultitenancyConstants.Identifier)
        ?? httpContextAccessor.HttpContext?.Request?.Headers[MultitenancyConstants.Identifier].FirstOrDefault()
        ?? httpContextAccessor.HttpContext?.Items["TenantId"] as string;
    public string? UserId => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? httpContextAccessor.HttpContext?.User?.FindFirstValue("sub");
    public string? UserName => httpContextAccessor.HttpContext?.User?.Identity?.Name ?? httpContextAccessor.HttpContext?.User?.FindFirstValue("name");
    public string? TraceId => Activity.Current?.TraceId.ToString();
    public string? SpanId => Activity.Current?.SpanId.ToString();
    public string? CorrelationId => httpContextAccessor.HttpContext?.TraceIdentifier;
    public string? RequestId => httpContextAccessor.HttpContext?.TraceIdentifier;
    public string? Source => httpContextAccessor.HttpContext?.GetEndpoint()?.DisplayName ?? "API";

    public AuditTag Tags => AuditTag.None;

    public IAuditScope WithTags(AuditTag tags) => this; // immutable view
    public IAuditScope WithProperties(string? tenantId = null, string? userId = null, string? userName = null, string? traceId = null,
        string? spanId = null, string? correlationId = null, string? requestId = null, string? source = null, AuditTag? tags = null) => this;
}
