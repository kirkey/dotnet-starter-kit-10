namespace FSH.Framework.Blazor.UI.Components.Data.EntityTable;

/// <summary>
/// Helper class for API calls with error handling.
/// </summary>
public static class ApiHelper
{
    public static async Task<T?> ExecuteCallGuardedAsync<T>(
        Func<Task<T>> call,
        FshValidation? customValidation = null,
        string? successMessage = null)
    {
        try
        {
            customValidation?.ClearErrors();
            var result = await call();
            
            // If we have a snackbar service and success message, show it
            // This would need to be injected through a service if needed
            
            return result;
        }
        catch (Exception ex)
        {
            // Handle validation errors
            if (customValidation is not null && ex.Data.Contains("ValidationErrors") &&
                ex.Data["ValidationErrors"] is IDictionary<string, ICollection<string>> errors)
            {
                customValidation.DisplayErrors(errors);
            }
            
            // Log or handle the exception as needed
            return default;
        }
    }
}
