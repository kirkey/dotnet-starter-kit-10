using Microsoft.AspNetCore.Components;

namespace FSH.Basic.Blazor.Services.Api;

/// <summary>
/// Global error handler service that handles 401 errors and automatically triggers logout.
/// </summary>
public interface IGlobalErrorHandler
{
    /// <summary>
    /// Handles an exception, checking if it's a 401 error and triggering logout if needed.
    /// </summary>
    Task HandleExceptionAsync(Exception exception);
}

internal sealed class GlobalErrorHandler(
    NavigationManager navigationManager,
    ILogger<GlobalErrorHandler> logger)
    : IGlobalErrorHandler
{
    private bool _isHandling401;

    public async Task HandleExceptionAsync(Exception exception)
    {
        // Prevent multiple simultaneous 401 handling
        if (_isHandling401)
        {
            return;
        }

        try
        {
            // Check if this is a 401 error from API client
            if (Is401Exception(exception))
            {
                _isHandling401 = true;
                logger.LogWarning("Detected 401 Unauthorized error, redirecting to login");
                
                // Use forceLoad to ensure server-side logout is triggered
                await Task.Run(() => 
                {
                    navigationManager.NavigateTo("/auth/logout?toast=session_expired", forceLoad: true);
                });
            }
        }
        finally
        {
            _isHandling401 = false;
        }
    }

    private static bool Is401Exception(Exception exception)
    {
        // Check if it's an ApiException with 401 status
        if (exception is ApiException apiException && apiException.StatusCode == 401)
        {
            return true;
        }

        // Check if it's an HttpRequestException with 401 message
        if (exception is HttpRequestException httpException)
        {
            string message = httpException.Message.ToLowerInvariant();
            if (message.Contains("401") || message.Contains("unauthorized"))
            {
                return true;
            }
        }

        // Check inner exception
        if (exception.InnerException != null)
        {
            return Is401Exception(exception.InnerException);
        }

        return false;
    }
}
