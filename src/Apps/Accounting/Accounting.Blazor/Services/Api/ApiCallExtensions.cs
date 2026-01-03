namespace FSH.Basic.Blazor.Services.Api;

/// <summary>
/// Extension methods for handling API calls with automatic 401 error handling.
/// </summary>
public static class ApiCallExtensions
{
    /// <summary>
    /// Executes an API call and automatically handles 401 errors by triggering logout.
    /// </summary>
    /// <typeparam name="T">The return type of the API call</typeparam>
    /// <param name="errorHandler">The global error handler</param>
    /// <param name="apiCall">The API call to execute</param>
    /// <param name="onError">Optional callback for other errors</param>
    /// <returns>The result of the API call, or default if an error occurred</returns>
    public static async Task<T?> ExecuteAsync<T>(
        this IGlobalErrorHandler errorHandler,
        Func<Task<T>> apiCall,
        Action<Exception>? onError = null)
    {
        try
        {
            return await apiCall();
        }
        catch (Exception ex)
        {
            await errorHandler.HandleExceptionAsync(ex);
            onError?.Invoke(ex);
            return default;
        }
    }

    /// <summary>
    /// Executes an API call that doesn't return a value and automatically handles 401 errors.
    /// </summary>
    /// <param name="errorHandler">The global error handler</param>
    /// <param name="apiCall">The API call to execute</param>
    /// <param name="onError">Optional callback for other errors</param>
    public static async Task ExecuteAsync(
        this IGlobalErrorHandler errorHandler,
        Func<Task> apiCall,
        Action<Exception>? onError = null)
    {
        try
        {
            await apiCall();
        }
        catch (Exception ex)
        {
            await errorHandler.HandleExceptionAsync(ex);
            onError?.Invoke(ex);
        }
    }
}
