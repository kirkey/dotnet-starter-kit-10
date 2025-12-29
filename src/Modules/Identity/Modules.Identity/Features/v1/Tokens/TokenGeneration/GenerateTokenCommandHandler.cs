using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Eventing.Outbox;
using FSH.Framework.Shared.Multitenancy;
using FSH.Modules.Auditing.Contracts;
using FSH.Modules.Identity.Contracts.DTOs;
using FSH.Modules.Identity.Contracts.Events;
using FSH.Modules.Identity.Contracts.Services;
using FSH.Modules.Identity.Contracts.v1.Tokens.TokenGeneration;
using Mediator;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FSH.Modules.Identity.Features.v1.Tokens.TokenGeneration;

public sealed class GenerateTokenCommandHandler(
    IIdentityService identityService,
    ITokenService tokenService,
    ISecurityAudit securityAudit,
    IHttpContextAccessor http,
    IOutboxStore outboxStore,
    IMultiTenantContextAccessor<AppTenantInfo> multiTenantContextAccessor,
    ISessionService sessionService)
    : ICommandHandler<GenerateTokenCommand, TokenResponse>
{
    public async ValueTask<TokenResponse> Handle(
        GenerateTokenCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Gather context for auditing
        HttpContext? http1 = http.HttpContext;
        string ip = http1?.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        string ua = http1?.Request.Headers.UserAgent.ToString() ?? "unknown";
        string? clientId = http1?.Request.Headers["X-Client-Id"].ToString();
        if (string.IsNullOrWhiteSpace(clientId)) clientId = "web";

        // Validate credentials
        (string Subject, IEnumerable<Claim> Claims)? identityResult = await identityService
            .ValidateCredentialsAsync(request.Email, request.Password, cancellationToken);

        if (identityResult is null)
        {
            // 1) Audit failed login BEFORE throwing
            await securityAudit.LoginFailedAsync(
                subjectIdOrName: request.Email,
                clientId: clientId!,
                reason: "InvalidCredentials",
                ip: ip,
                ct: cancellationToken);

            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        // Unpack subject + claims
        (string subject, IEnumerable<Claim> claims) = identityResult.Value;

        // 2) Audit successful login
        await securityAudit.LoginSucceededAsync(
            userId: subject,
            userName: claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? request.Email,
            clientId: clientId!,
            ip: ip,
            userAgent: ua,
            ct: cancellationToken);

        // Issue token
        TokenResponse token = await tokenService.IssueAsync(subject, claims, /*extra*/ null, cancellationToken);

        // Persist refresh token (hashed) for this user
        await identityService.StoreRefreshTokenAsync(subject, token.RefreshToken, token.RefreshTokenExpiresAt, cancellationToken);

        // Create user session for session management (non-blocking, fail gracefully)
        try
        {
            string refreshTokenHash = Sha256Short(token.RefreshToken);
            await sessionService.CreateSessionAsync(
                subject,
                refreshTokenHash,
                ip,
                ua,
                token.RefreshTokenExpiresAt,
                cancellationToken);
        }
        catch (Exception)
        {
            // Session creation is non-critical - don't fail the login
            // This can happen if migrations haven't been applied yet
        }

        // 3) Audit token issuance with a fingerprint (never raw token)
        string fingerprint = Sha256Short(token.AccessToken);
        await securityAudit.TokenIssuedAsync(
            userId: subject,
            userName: claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? request.Email,
            clientId: clientId!,
            tokenFingerprint: fingerprint,
            expiresUtc: token.AccessTokenExpiresAt,
            ct: cancellationToken);

        // 4) Enqueue integration event for token generation (sample event for testing eventing)
        string? tenantId = multiTenantContextAccessor.MultiTenantContext?.TenantInfo?.Id;
        string correlationId = Guid.NewGuid().ToString();

        TokenGeneratedIntegrationEvent integrationEvent = new(
            Id: Guid.NewGuid(),
            OccurredOnUtc: DateTime.UtcNow,
            TenantId: tenantId,
            CorrelationId: correlationId,
            Source: "Identity",
            UserId: subject,
            Email: request.Email,
            ClientId: clientId!,
            IpAddress: ip,
            UserAgent: ua,
            TokenFingerprint: fingerprint,
            AccessTokenExpiresAtUtc: token.AccessTokenExpiresAt);

        await outboxStore.AddAsync(integrationEvent, cancellationToken).ConfigureAwait(false);

        return token;
    }

    private static string Sha256Short(string value)
    {
        byte[] hash = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(value));
        // short printable fingerprint; store only this
        return Convert.ToHexString(hash.AsSpan(0, 8));
    }
}
