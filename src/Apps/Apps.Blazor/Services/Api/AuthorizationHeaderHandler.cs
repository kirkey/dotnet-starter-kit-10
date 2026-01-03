using Microsoft.AspNetCore.Authentication;
using System.Net;

namespace FSH.Apps.Blazor.Services.Api;

/// <summary>
/// Delegating handler that adds the JWT token to API requests and handles 401 responses
/// by attempting to refresh the access token. If refresh fails, signs out the user.
/// </summary>
internal sealed class AuthorizationHeaderHandler(
    IHttpContextAccessor httpContextAccessor,
    IServiceProvider serviceProvider,
    ILogger<AuthorizationHeaderHandler> logger)
    : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Attach current access token
        string? accessToken = await GetAccessTokenAsync();
        if (!string.IsNullOrEmpty(accessToken))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
        }

        // Send the request
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        // If we get a 401, try to refresh the token and retry once
        if (response.StatusCode == HttpStatusCode.Unauthorized && !string.IsNullOrEmpty(accessToken))
        {
            logger.LogInformation("Received 401 response, attempting token refresh");

            string? newAccessToken = await TryRefreshTokenAsync(cancellationToken);

            if (!string.IsNullOrEmpty(newAccessToken))
            {
                logger.LogInformation("Token refresh successful, retrying request");

                // Clone the request with new token
                using HttpRequestMessage retryRequest = await CloneHttpRequestMessageAsync(request);
                retryRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", newAccessToken);

                // Dispose the original response before retrying
                response.Dispose();

                // Retry the request with the new token
                response = await base.SendAsync(retryRequest, cancellationToken);
            }
            else
            {
                logger.LogWarning("Token refresh failed, signing out user and returning 401 response");

                // Sign out the user since refresh token is also invalid/expired
                await SignOutUserAsync();
            }
        }

        return response;
    }

    private async Task SignOutUserAsync()
    {
        try
        {
            HttpContext? httpContext = httpContextAccessor.HttpContext;
            if (httpContext is not null)
            {
                await httpContext.SignOutAsync("Cookies");
                logger.LogInformation("User signed out due to expired refresh token");

                // For SSR requests, redirect to login
                // For API/fetch requests, the client will handle the redirect
                if (!httpContext.Response.HasStarted && 
                    !httpContext.Request.Headers.ContainsKey("X-Requested-With"))
                {
                    httpContext.Response.Redirect("/login?toast=session_expired");
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to sign out user after token refresh failure");
        }
    }

    private async Task<string?> GetAccessTokenAsync()
    {
        try
        {
            HttpContext? httpContext = httpContextAccessor.HttpContext;
            ClaimsPrincipal? user = httpContext?.User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                return user.FindFirst("access_token")?.Value;
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Failed to get access token from claims");
        }

        return null;
    }

    private async Task<string?> TryRefreshTokenAsync(CancellationToken cancellationToken)
    {
        try
        {
            // Resolve the token refresh service from the service provider
            // We use IServiceProvider to avoid circular dependency issues
            ITokenRefreshService? tokenRefreshService = serviceProvider.GetService<ITokenRefreshService>();
            if (tokenRefreshService is null)
            {
                logger.LogWarning("TokenRefreshService is not registered");
                return null;
            }

            return await tokenRefreshService.TryRefreshTokenAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during token refresh");
            return null;
        }
    }

    private static async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(HttpRequestMessage request)
    {
        HttpRequestMessage clone = new(request.Method, request.RequestUri)
        {
            Version = request.Version
        };

        // Copy headers (except Authorization which we'll set separately)
        foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers.Where(h => !string.Equals(h.Key, "Authorization", StringComparison.OrdinalIgnoreCase)))
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        // Copy content if present
        if (request.Content != null)
        {
            byte[] contentBytes = await request.Content.ReadAsByteArrayAsync();
            clone.Content = new ByteArrayContent(contentBytes);

            // Copy content headers
            foreach (KeyValuePair<string, IEnumerable<string>> header in request.Content.Headers)
            {
                clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        // Copy options
        foreach (KeyValuePair<string, object?> option in request.Options)
        {
            clone.Options.TryAdd(option.Key, option.Value);
        }

        return clone;
    }
}
