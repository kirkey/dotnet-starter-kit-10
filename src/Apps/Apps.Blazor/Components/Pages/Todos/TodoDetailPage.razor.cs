using FSH.Apps.Blazor.Api;
using FSH.Apps.Blazor.Services.Api;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FSH.Apps.Blazor.Components.Pages.Todos;

public partial class TodoDetailPage
{
    [Parameter] public Guid Id { get; set; }

    private TodoDto? _todo;
    private bool _loading = true;
    private bool _busy;

    protected override async Task OnInitializedAsync()
    {
        await LoadTodo();
    }

    private async Task LoadTodo()
    {
        _loading = true;
        try
        {
            _todo = await ErrorHandler.ExecuteAsync(
                async () => await TodoClient.TodoGetAsync(Id),
                ex => Snackbar.Add($"Failed to load todo: {ex.Message}", Severity.Error));
        }
        finally
        {
            _loading = false;
        }
    }

    private async Task RefreshTodo()
    {
        await LoadTodo();
    }

    private void GoBack()
    {
        Navigation.NavigateTo("/todos");
    }

    private async Task EditTodo()
    {
        if (_todo is null) return;

        var parameters = new DialogParameters
        {
            { "TodoId", _todo.Id },
            { "Name", _todo.Name },
            { "Description", _todo.Description },
            { "Priority", _todo.Priority },
            { "DueDate", _todo.DueDate }
        };

        var dialog = await DialogService.ShowAsync<CreateTodoListDialog>("Edit Todo", parameters, new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true
        });

        var result = await dialog.Result;
        if (!result.Canceled)
        {
            await RefreshTodo();
        }
    }

    private async Task CompleteTodo()
    {
        if (_todo is null) return;

        var confirmed = await DialogService.ShowMessageBox(
            "Complete Todo",
            "Are you sure you want to mark this todo as complete?",
            yesText: "Complete",
            cancelText: "Cancel");

        if (confirmed != true) return;

        _busy = true;
        try
        {
            await ErrorHandler.ExecuteAsync(
                async () => await TodoClient.TodoCompleteAsync(_todo.Id),
                ex => Snackbar.Add($"Failed to complete todo: {ex.Message}", Severity.Error));

            Snackbar.Add("Todo completed successfully", Severity.Success);
            await RefreshTodo();
        }
        finally
        {
            _busy = false;
        }
    }

    private async Task ReopenTodo()
    {
        if (_todo is null) return;

        _busy = true;
        try
        {
            await ErrorHandler.ExecuteAsync(
                async () => await TodoClient.TodoReopenAsync(_todo.Id),
                ex => Snackbar.Add($"Failed to reopen todo: {ex.Message}", Severity.Error));

            Snackbar.Add("Todo reopened successfully", Severity.Success);
            await RefreshTodo();
        }
        finally
        {
            _busy = false;
        }
    }

    private async Task DeleteTodo()
    {
        if (_todo is null) return;

        var confirmed = await DialogService.ShowMessageBox(
            "Delete Todo",
            $"Are you sure you want to delete '{_todo.Name}'? This action cannot be undone.",
            yesText: "Delete",
            cancelText: "Cancel");

        if (confirmed != true) return;

        _busy = true;
        try
        {
            await ErrorHandler.ExecuteAsync(
                async () => await TodoClient.TodoDeleteAsync(_todo.Id),
                ex => Snackbar.Add($"Failed to delete todo: {ex.Message}", Severity.Error));

            Snackbar.Add("Todo deleted successfully", Severity.Success);
            Navigation.NavigateTo("/todos");
        }
        finally
        {
            _busy = false;
        }
    }

    private static int GetCompletionPercentage(int totalTasks, int completedTasks)
    {
        if (totalTasks == 0) return 0;
        return (int)Math.Round((double)completedTasks / totalTasks * 100);
    }

    private static Color GetStatusColor(string status)
    {
        return status?.ToLowerInvariant() switch
        {
            "inprogress" => Color.Info,
            "completed" => Color.Success,
            "archived" => Color.Default,
            _ => Color.Primary
        };
    }

    private static Color GetPriorityColor(int priority)
    {
        return priority switch
        {
            3 => Color.Error, // Critical
            2 => Color.Warning, // High
            1 => Color.Info, // Medium
            _ => Color.Default // Low
        };
    }

    private static string GetPriorityText(int priority)
    {
        return priority switch
        {
            3 => "Critical",
            2 => "High",
            1 => "Medium",
            _ => "Low"
        };
    }

    private static Color GetDueDateColor(DateTimeOffset dueDate)
    {
        var daysUntilDue = (dueDate - DateTimeOffset.UtcNow).Days;
        return daysUntilDue switch
        {
            < 0 => Color.Error,
            < 3 => Color.Warning,
            _ => Color.Info
        };
    }
}
