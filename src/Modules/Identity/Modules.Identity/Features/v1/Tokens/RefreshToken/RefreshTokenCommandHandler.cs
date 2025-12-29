using FSH.Modules.Auditing.Contracts;
using FSH.Modules.Identity.Contracts.DTOs;
using FSH.Modules.Identity.Contracts.Services;
using FSH.Modules.Identity.Contracts.v1.Tokens.RefreshToken;
using Mediator;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FSH.Modules.Identity.Features.v1.Tokens.RefreshToken;

public sealed class RefreshTokenCommandHandler(
    IIdentityService identityService,
    ITokenService tokenService,
    ISecurityAudit securityAudit,
    IHttpContextAccessor http,
    ISessionService sessionService)
    : ICommandHandler<RefreshTokenCommand, RefreshTokenCommandResponse>
{
    public async ValueTask<RefreshTokenCommandResponse> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        HttpContext? http1 = http.HttpContext;
        string? clientId = http1?.Request.Headers["X-Client-Id"].ToString();
        if (string.IsNullOrWhiteSpace(clientId)) clientId = "web";

        // Validate refresh token and rebuild subject + claims
        (string Subject, IEnumerable<Claim> Claims)? validated = await identityService
            .ValidateRefreshTokenAsync(request.RefreshToken, cancellationToken);

        if (validated is null)
        {
            await securityAudit.TokenRevokedAsync("unknown", clientId!, "InvalidRefreshToken", cancellationToken);
            throw new UnauthorizedAccessException("Invalid refresh token.");
        }

        (string subject, IEnumerable<Claim> claims) = validated.Value;

        // Check if the session associated with this refresh token is still valid
        string refreshTokenHash = Sha256Short(request.RefreshToken);
        bool isSessionValid = await sessionService.ValidateSessionAsync(refreshTokenHash, cancellationToken);
        if (!isSessionValid)
        {
            await securityAudit.TokenRevokedAsync(subject, clientId!, "SessionRevoked", cancellationToken);
            throw new UnauthorizedAccessException("Session has been revoked.");
        }

        // Optionally, cross-check the provided access token subject
        JwtSecurityTokenHandler handler = new();
        JwtSecurityToken? parsedAccessToken = null;
        try
        {
            parsedAccessToken = handler.ReadJwtToken(request.Token);
        }
        catch
        {
            // Ignore parsing errors and rely on refresh-token validation
        }

        if (parsedAccessToken is not null)
        {
            string? accessTokenSubject = parsedAccessToken.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(accessTokenSubject) &&
                !string.Equals(accessTokenSubject, subject, StringComparison.Ordinal))
            {
                await securityAudit.TokenRevokedAsync(subject, clientId!, "RefreshTokenSubjectMismatch", cancellationToken);
                throw new UnauthorizedAccessException("Access token subject mismatch.");
            }
        }

        // Audit previous token revocation by rotation (no raw tokens)
        await securityAudit.TokenRevokedAsync(subject, clientId!, "RefreshTokenRotated", cancellationToken);

        // Issue new tokens
        TokenResponse newToken = await tokenService.IssueAsync(subject, claims, null, cancellationToken);

        // Persist rotated refresh token for this user
        await identityService.StoreRefreshTokenAsync(subject, newToken.RefreshToken, newToken.RefreshTokenExpiresAt, cancellationToken);

        // Update the session with the new refresh token hash
        string newRefreshTokenHash = Sha256Short(newToken.RefreshToken);
        await sessionService.UpdateSessionRefreshTokenAsync(
            refreshTokenHash,
            newRefreshTokenHash,
            newToken.RefreshTokenExpiresAt,
            cancellationToken);

        // Audit the newly issued token with a fingerprint
        string fingerprint = Sha256Short(newToken.AccessToken);
        await securityAudit.TokenIssuedAsync(
            userId: subject,
            userName: claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? string.Empty,
            clientId: clientId!,
            tokenFingerprint: fingerprint,
            expiresUtc: newToken.AccessTokenExpiresAt,
            ct: cancellationToken);

        return new RefreshTokenCommandResponse(
            Token: newToken.AccessToken,
            RefreshToken: newToken.RefreshToken,
            RefreshTokenExpiryTime: newToken.RefreshTokenExpiresAt);
    }

    private static string Sha256Short(string value)
    {
        byte[] hash = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(hash.AsSpan(0, 8));
    }
}
