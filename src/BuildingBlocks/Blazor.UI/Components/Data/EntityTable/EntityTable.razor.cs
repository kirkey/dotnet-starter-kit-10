using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace FSH.Framework.Blazor.UI.Components.Data.EntityTable;

public partial class EntityTable<TEntity, TId, TRequest>
    where TRequest : new()
{
    [Parameter]
    [EditorRequired]
    public EntityTableContext<TEntity, TId, TRequest> Context { get; set; } = null!;

    [Parameter] public bool Exporting { get; set; }
    [Parameter] public bool Importing { get; set; }
    [Parameter] public bool Loading { get; set; }
    [Parameter] public string? SearchString { get; set; }
    [Parameter] public int[] PageSizes { get; set; } = [10, 20, 50, 100, 500, 999];
    [Parameter] public EventCallback<string> SearchStringChanged { get; set; }
    [Parameter] public RenderFragment? AdvancedSearchContent { get; set; }
    [Parameter] public RenderFragment<TEntity>? ActionsContent { get; set; }
    [Parameter] public RenderFragment<TEntity>? ExtraActions { get; set; }
    [Parameter] public RenderFragment<TEntity>? ChildRowContent { get; set; }
    [Parameter] public RenderFragment<TRequest>? EditFormContent { get; set; }

    [CascadingParameter] protected Task<AuthenticationState> AuthState { get; set; } = null!;
    
    [Inject] protected IAuthorizationService? AuthService { get; set; }
    [Inject] protected IDialogService DialogService { get; set; } = null!;
    [Inject] protected ISnackbar Snackbar { get; set; } = null!;
    [Inject] protected IJSRuntime Js { get; set; } = null!;

    private bool _canSearch;
    private bool _canCreate;
    private bool _canUpdate;
    private bool _canDelete;
    private bool _canImport;
    private bool _canExport;

    private bool _advancedSearchExpanded;

    private MudTable<TEntity> _table = null!;
    private IEnumerable<TEntity>? _entityList;
    private int _totalItems;

    private bool _initialized;

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        
        if (!_initialized && Context is not null)
        {
            _initialized = true;
            await InitializeAsync();
        }
    }

    protected override async Task OnInitializedAsync()
    {
        if (Context is null)
        {
            return;
        }

        if (!_initialized)
        {
            _initialized = true;
            await InitializeAsync();
        }
    }

    private async Task InitializeAsync()
    {
        var state = await AuthState;
        _canSearch = await CanDoActionAsync(Context.SearchAction, state);
        _canCreate = await CanDoActionAsync(Context.CreateAction, state);
        _canUpdate = await CanDoActionAsync(Context.UpdateAction, state);
        _canDelete = await CanDoActionAsync(Context.DeleteAction, state);
        _canImport = await CanDoActionAsync(Context.ImportAction, state);
        _canExport = await CanDoActionAsync(Context.ExportAction, state);

        await LocalLoadDataAsync();
    }

    public Task ReloadDataAsync() =>
        Context.IsClientContext
            ? LocalLoadDataAsync()
            : ServerLoadDataAsync();

    private async Task<bool> CanDoActionAsync(string? action, AuthenticationState state)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(action))
                return false;
                
            // Check if action equals "True", then it's allowed
            if (bool.TryParse(action, out bool isTrue) && isTrue)
                return true;
                
            // Check authorization if AuthService is available and resource is defined
            if (AuthService is not null && Context.EntityResource is { } resource)
            {
                var authResult = await AuthService.AuthorizeAsync(state.User, resource, action);
                return authResult.Succeeded;
            }
            
            return false;
        }
        catch
        {
            return false;
        }
    }

    private bool HasActions => _canUpdate || _canDelete || Context.HasExtraActionsFunc is not null && Context.HasExtraActionsFunc();
    private bool CanUpdateEntity(TEntity entity) => _canUpdate && Context.UpdateFunc is not null && (Context.CanUpdateEntityFunc is null || Context.CanUpdateEntityFunc(entity));
    private bool CanDeleteEntity(TEntity entity) => _canDelete && Context.DeleteFunc is not null && (Context.CanDeleteEntityFunc is null || Context.CanDeleteEntityFunc(entity));

    private bool LocalSearch(TEntity entity) =>
        Context.ClientContext?.SearchFunc is { } searchFunc
            ? searchFunc(SearchString, entity)
            : string.IsNullOrWhiteSpace(SearchString);

    private async Task LocalLoadDataAsync()
    {
        if (Loading || Context.ClientContext is null)
        {
            return;
        }

        Loading = true;

        try
        {
            _entityList = await Context.ClientContext.LoadDataFunc() ?? [];
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading data: {ex.Message}", Severity.Error);
            _entityList = [];
        }

        Loading = false;
    }

    private async Task OnSearchStringChanged(string? text = null)
    {
        await SearchStringChanged.InvokeAsync(SearchString);
        await ServerLoadDataAsync();
    }

    private async Task ServerLoadDataAsync()
    {
        if (Context.IsServerContext)
        {
            await _table.ReloadServerData();
        }
    }

    private Func<TableState, CancellationToken, Task<TableData<TEntity>>>? ServerReloadFunc =>
        Context?.IsServerContext == true ? ServerReload : null;

    private async Task<TableData<TEntity>> ServerReload(TableState state, CancellationToken cancellationToken)
    {
        if (Loading || Context.ServerContext is null)
        {
            return new TableData<TEntity> { TotalItems = _totalItems, Items = _entityList };
        }

        Loading = true;

        var filter = GetPaginationFilter(state);

        try
        {
            var result = await Context.ServerContext.SearchFunc(filter);
            _totalItems = result.TotalCount;
            _entityList = result.Items;
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Error loading data: {ex.Message}", Severity.Error);
        }

        Loading = false;

        return new TableData<TEntity> { TotalItems = _totalItems, Items = _entityList };
    }

    private PaginationFilter GetPaginationFilter(TableState state)
    {
        string[]? orderings = null;
        if (!string.IsNullOrEmpty(state.SortLabel))
        {
            orderings = state.SortDirection == SortDirection.None
                ? [$"{state.SortLabel}"]
                : [$"{state.SortLabel} {state.SortDirection}"];
        }

        var filter = new PaginationFilter
        {
            PageSize = state.PageSize,
            PageNumber = state.Page + 1,
            Keyword = SearchString,
            OrderBy = orderings ?? []
        };

        if (Context.AllColumnsChecked)
        {
            return filter;
        }

        filter.AdvancedSearch = new Search
        {
            Fields = Context.SearchFields,
        };
        filter.Keyword = null;

        return filter;
    }

    private async Task InvokeModal(TEntity? entity = default, TEntity? entityToDuplicate = default, bool isViewMode = false)
    {
        bool isCreate = entity is null && !isViewMode;
    
        var parameters = new DialogParameters
        {
            { nameof(AddEditModal<TRequest>.ChildContent), EditFormContent },
            { nameof(AddEditModal<TRequest>.OnInitializedFunc), Context.EditFormInitializedFunc },
            { nameof(AddEditModal<TRequest>.IsCreate), isCreate },
            { nameof(AddEditModal<TRequest>.IsViewMode), isViewMode }
        };
    
        Func<TRequest, Task> saveFunc;
        if (isCreate)
        {
            saveFunc = Context.CreateFunc ?? throw new InvalidOperationException("CreateFunc can't be null!");
        }
        else
        {
            saveFunc = request => Context.UpdateFunc!(Context.IdFunc!(entity!), request);
        }
    
        TRequest requestModel;
        if (isCreate || entityToDuplicate is not null)
        {
            requestModel = await GetRequestModel(entityToDuplicate).ConfigureAwait(false);
        }
        else if (Context.GetDetailsFunc is not null)
        {
            requestModel = await Context.GetDetailsFunc(Context.IdFunc!(entity!));
        }
        else
        {
            requestModel = (TRequest)(object)entity!;
        }
    
        string title;
        if (isViewMode)
            title = $"View {Context.EntityName}";
        else if (isCreate)
            title = $"Create {Context.EntityName}";
        else
            title = $"Edit {Context.EntityName}";
            
        string successMessage = isCreate 
            ? $"{Context.EntityName} Created" 
            : $"{Context.EntityName} Updated";
    
        parameters.Add(nameof(AddEditModal<TRequest>.SaveFunc), saveFunc);
        parameters.Add(nameof(AddEditModal<TRequest>.RequestModel), requestModel);
        parameters.Add(nameof(AddEditModal<TRequest>.Title), title);
        parameters.Add(nameof(AddEditModal<TRequest>.SuccessMessage), successMessage);
    
        var dialogOptions = new DialogOptions
        {
            CloseButton = true,
            MaxWidth = MaxWidth.Large,
            FullWidth = true,
            BackdropClick = false
        };
        
        var dialog = await DialogService.ShowAsync<AddEditModal<TRequest>>(title, parameters, dialogOptions);
    
        Context.SetAddEditModalRef(dialog);
    
        var result = await dialog.Result;
    
        if (!result!.Canceled)
        {
            await ReloadDataAsync();
        }
    }
    
    private async Task<TRequest> GetRequestModel(TEntity? entityToDuplicate)
    {
        if (entityToDuplicate is not null)
        {
            if (Context.GetDuplicateFunc is not null)
            {
                return await Context.GetDuplicateFunc(entityToDuplicate);
            }
            return (TRequest)(object)entityToDuplicate;
        }

        if (Context.GetDefaultsFunc is not null)
        {
            return await Context.GetDefaultsFunc();
        }

        return new TRequest();
    }

    private async Task Delete(TEntity entity)
    {
        _ = Context.IdFunc ?? throw new InvalidOperationException("IdFunc can't be null!");
        TId id = Context.IdFunc(entity);

        string deleteContent = "You're sure you want to delete {0} with id '{1}'?";
        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), string.Format(deleteContent, Context.EntityName, id) }
        };
        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
        var dialog = await DialogService.ShowAsync<DeleteConfirmation>("Delete", parameters, options);
        var result = await dialog.Result;
        if (!result!.Canceled)
        {
            _ = Context.DeleteFunc ?? throw new InvalidOperationException("DeleteFunc can't be null!");

            try
            {
                await Context.DeleteFunc(id);
                Snackbar.Add($"{Context.EntityName} deleted successfully", Severity.Success);
                await ReloadDataAsync();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error deleting {Context.EntityName}: {ex.Message}", Severity.Error);
            }
        }
    }
    
    private async Task ExportAsync()
    {
        await Task.Yield();

        if (!Exporting)
        {
            if (Context.ServerContext?.ExportFunc != null)
            {
                const string action = "Export";
                const string content = "You're sure you want to export '{0}'?";
                var parameters = new DialogParameters
                {
                    { nameof(TransactionConfirmation.ContentText), string.Format(content, Context.EntityNamePlural) },
                    { nameof(TransactionConfirmation.ConfirmText), action }
                };
                var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, BackdropClick = false };
                var dialog = await DialogService.ShowAsync<TransactionConfirmation>($"{action} {Context.EntityNamePlural}", parameters, options);
                var result = await dialog.Result;
                if (!result!.Canceled)
                {
                    Exporting = true;
                    var filter = GetBaseFilter();
                    try
                    {
                        var response = await Context.ServerContext.ExportFunc(filter);
                        using var streamRef = new DotNetStreamReference(response.Stream);
                        await Js.InvokeVoidAsync("downloadFileFromStream", $"{Context.EntityNamePlural}.xlsx", streamRef);
                    }
                    catch (Exception ex)
                    {
                        Snackbar.Add($"Error exporting: {ex.Message}", Severity.Error);
                    }
                    Exporting = false;
                }
            }
        }
    }
    
    private PaginationFilter GetBaseFilter()
    {
        var filter = new PaginationFilter
        {
            Keyword = SearchString
        };

        if (!Context.AllColumnsChecked)
        {
            filter.AdvancedSearch = new Search
            {
                Fields = Context.SearchFields,
            };
            filter.Keyword = null;
        }

        return filter;
    }
}
