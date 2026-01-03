using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;

namespace FSH.Basic.Blazor.Services;

#pragma warning disable CA1515 // Extension method classes must be public
internal static class SimpleBffAuth
#pragma warning restore CA1515
{
    public static void MapSimpleBffAuthEndpoints(this WebApplication app)
    {
        // Login endpoint - calls identity API, sets cookie, returns success
        // Note: Uses /bff/ prefix to avoid conflict with ALB routing /api/* to the API service
        app.MapPost("/bff/auth/login", async (
            HttpContext httpContext,
            ITokenClient tokenClient,
            ILogger<Program> logger) =>
        {
            try
            {
                // Read form data
                IFormCollection form = await httpContext.Request.ReadFormAsync();
                string email = form["Email"].ToString();
                string password = form["Password"].ToString();
                string? tenant = form["Tenant"].ToString();

                logger.LogInformation("Login attempt for {Email}", email);

                // Call the identity API to get token
                TokenResponse? token = await tokenClient.IssueAsync(
                    tenant ?? "root",
                    new GenerateTokenCommand
                    {
                        Email = email,
                        Password = password
                    });

                if (token == null || string.IsNullOrEmpty(token.AccessToken))
                {
                    return Results.Unauthorized();
                }

                // Parse JWT to extract claims
                JwtSecurityTokenHandler jwtHandler = new();
                JwtSecurityToken? jwtToken = jwtHandler.ReadJwtToken(token.AccessToken);

                List<Claim> claims = new()
                {
                    new(ClaimTypes.NameIdentifier, jwtToken.Subject ?? Guid.NewGuid().ToString()),
                    new(ClaimTypes.Email, email),
                    new("access_token", token.AccessToken), // Store JWT for API calls
                    new("refresh_token", token.RefreshToken), // Store refresh token for token renewal
                    new("tenant", tenant ?? "root"), // Store tenant for token refresh
                };

                // Add name claim
                Claim? nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "name" || c.Type == ClaimTypes.Name);
                if (nameClaim != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, nameClaim.Value));
                }

                // Add role claims
                IEnumerable<Claim> roleClaims = jwtToken.Claims.Where(c => c.Type == "role" || c.Type == ClaimTypes.Role);
                claims.AddRange(roleClaims.Select(r => new Claim(ClaimTypes.Role, r.Value)));

                // Create identity and sign in with cookie
                ClaimsIdentity identity = new(claims, "Cookies");
                ClaimsPrincipal principal = new(identity);

                await httpContext.SignInAsync("Cookies", principal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                });

                logger.LogInformation("Login successful for {Email}", email);

                // Redirect to home page - this ensures the cookie is properly read on the next request
                return Results.Redirect("/");
            }
            catch (ApiException ex) when (ex.StatusCode == 401)
            {
                return Results.Unauthorized();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Login failed");
                return Results.Problem("Login failed");
            }
        })
        .AllowAnonymous()
        .DisableAntiforgery();

        // Logout endpoint - POST for API calls
        app.MapPost("/bff/auth/logout", async (HttpContext httpContext) =>
        {
            await httpContext.SignOutAsync("Cookies");
            return Results.Ok();
        })
        .DisableAntiforgery();

        // Logout endpoint - GET for browser redirects (ensures cookie is cleared in browser)
        app.MapGet("/auth/logout", async (HttpContext httpContext) =>
        {
            await httpContext.SignOutAsync("Cookies");
            return Results.Redirect("/login?toast=logout_success");
        })
        .AllowAnonymous();
    }
}
