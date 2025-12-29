using FSH.Modules.Identity.Authorization.Jwt;
using FSH.Modules.Identity.Contracts.DTOs;
using FSH.Modules.Identity.Contracts.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FSH.Modules.Identity.Services;

public sealed class TokenService : ITokenService
{
    private readonly JwtOptions _options;
    private readonly ILogger<TokenService> _logger;
    private readonly IdentityMetrics _metrics;

    public TokenService(IOptions<JwtOptions> options, ILogger<TokenService> logger, IdentityMetrics metrics)
    {
        ArgumentNullException.ThrowIfNull(options);
        _options = options.Value;
        _logger = logger;
        _metrics = metrics;
    }

    public Task<TokenResponse> IssueAsync(
        string subject,
        IEnumerable<Claim> claims,
        string? tenant = null,
        CancellationToken ct = default)
    {
        SymmetricSecurityKey signingKey = new(Encoding.UTF8.GetBytes(_options.SigningKey));
        SigningCredentials creds = new(signingKey, SecurityAlgorithms.HmacSha256);

        // Access token
        DateTime accessTokenExpiry = DateTime.UtcNow.AddMinutes(_options.AccessTokenMinutes);
        JwtSecurityToken jwtToken = new(
            _options.Issuer,
            _options.Audience,
            claims,
            expires: accessTokenExpiry,
            signingCredentials: creds);

        string? accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        // Refresh token
        string refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        DateTime refreshTokenExpiry = DateTime.UtcNow.AddDays(_options.RefreshTokenDays);

        string userEmail = claims.Where(a => a.Type == ClaimTypes.Email).Select(a => a.Value).First();
        _logger.LogInformation("Issued JWT for {Email}", userEmail);
        _metrics.TokenGenerated(userEmail);

        TokenResponse response = new(
            AccessToken: accessToken,
            RefreshToken: refreshToken,
            RefreshTokenExpiresAt: refreshTokenExpiry,
            AccessTokenExpiresAt: accessTokenExpiry);

        return Task.FromResult(response);
    }
}