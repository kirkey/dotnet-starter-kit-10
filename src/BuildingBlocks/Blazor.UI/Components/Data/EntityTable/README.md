# EntityTable Component

A comprehensive, reusable Blazor data table component with built-in CRUD operations, pagination, searching, sorting, and export capabilities.

## Features

- **Client-side and Server-side pagination** - Choose between local or remote data loading
- **Advanced search** - Column-specific search with keyword filtering
- **CRUD operations** - Create, Read, Update, Delete with modal dialogs
- **Sorting** - Column sorting support
- **Export** - Export data to Excel
- **Authorization** - Built-in permission checking
- **Customizable** - Flexible column templates and action menus

## Basic Usage

### 1. Client-Side Table

For small datasets that can be loaded entirely in the browser:

```csharp
@page "/products"
@using FSH.Framework.Blazor.UI.Components.Data.EntityTable

<EntityTable TEntity="Product" 
             TId="Guid" 
             TRequest="ProductRequest" 
             Context="@_context"
             EditFormContent="@EditForm"
             SearchString="@_searchString"
             @bind-Loading="_loading" />

@code {
    private EntityClientTableContext<Product, Guid, ProductRequest> _context = null!;
    private string? _searchString;
    private bool _loading;

    protected override void OnInitialized()
    {
        _context = new EntityClientTableContext<Product, Guid, ProductRequest>(
            fields: new List<EntityField<Product>>
            {
                new(p => p.Name, "Name", "Name", typeof(string)),
                new(p => p.Price, "Price", "Price", typeof(decimal)),
                new(p => p.IsActive, "Active", "IsActive", typeof(bool))
            },
            loadDataFunc: LoadProductsAsync,
            searchFunc: (searchText, product) =>
                string.IsNullOrWhiteSpace(searchText) ||
                product.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase),
            idFunc: p => p.Id,
            createFunc: CreateProductAsync,
            updateFunc: UpdateProductAsync,
            deleteFunc: DeleteProductAsync,
            entityName: "Product",
            entityNamePlural: "Products"
        );
    }

    private async Task<List<Product>?> LoadProductsAsync()
    {
        // Load your data here
        return await ProductService.GetAllAsync();
    }

    private async Task CreateProductAsync(ProductRequest request)
    {
        await ProductService.CreateAsync(request);
    }

    private async Task UpdateProductAsync(Guid id, ProductRequest request)
    {
        await ProductService.UpdateAsync(id, request);
    }

    private async Task DeleteProductAsync(Guid id)
    {
        await ProductService.DeleteAsync(id);
    }

    private RenderFragment<ProductRequest> EditForm => request => __builder =>
    {
        <MudItem xs="12">
            <MudTextField @bind-Value="request.Name" Label="Name" Required />
        </MudItem>
        <MudItem xs="12" sm="6">
            <MudNumericField @bind-Value="request.Price" Label="Price" Required />
        </MudItem>
        <MudItem xs="12" sm="6">
            <MudSwitch @bind-Value="request.IsActive" Label="Active" Color="Color.Primary" />
        </MudItem>
    };
}
```

### 2. Server-Side Table

For large datasets with server-side pagination and filtering:

```csharp
@page "/customers"
@using FSH.Framework.Blazor.UI.Components.Data.EntityTable

<EntityTable TEntity="Customer" 
             TId="Guid" 
             TRequest="CustomerRequest" 
             Context="@_context"
             EditFormContent="@EditForm"
             SearchString="@_searchString"
             @bind-Loading="_loading" />

@code {
    private EntityServerTableContext<Customer, Guid, CustomerRequest> _context = null!;
    private string? _searchString;
    private bool _loading;

    protected override void OnInitialized()
    {
        _context = new EntityServerTableContext<Customer, Guid, CustomerRequest>(
            fields: new List<EntityField<Customer>>
            {
                new(c => c.FirstName, "First Name", "FirstName", typeof(string)),
                new(c => c.LastName, "Last Name", "LastName", typeof(string)),
                new(c => c.Email, "Email", "Email", typeof(string)),
                new(c => c.CreatedOn, "Created", "CreatedOn", typeof(DateTime))
            },
            searchFunc: SearchCustomersAsync,
            enableAdvancedSearch: true,
            idFunc: c => c.Id,
            createFunc: CreateCustomerAsync,
            updateFunc: UpdateCustomerAsync,
            deleteFunc: DeleteCustomerAsync,
            exportFunc: ExportCustomersAsync,
            entityName: "Customer",
            entityNamePlural: "Customers"
        );
    }

    private async Task<PaginationResponse<Customer>> SearchCustomersAsync(PaginationFilter filter)
    {
        return await CustomerService.SearchAsync(filter);
    }

    private async Task<FileResponse> ExportCustomersAsync(PaginationFilter filter)
    {
        var stream = await CustomerService.ExportAsync(filter);
        return new FileResponse(stream);
    }

    // Other CRUD methods...
}
```

## Column Configuration

### Basic Column

```csharp
new EntityField<Product>(
    p => p.Name,           // Value selector
    "Product Name",        // Display name
    "Name",               // Sort label (for server-side)
    typeof(string)        // Type (affects rendering)
)
```

### Custom Column Template

```csharp
new EntityField<Product>(
    p => p.Status,
    "Status",
    "Status",
    Template: product => __builder =>
    {
        var color = product.Status == "Active" ? Color.Success : Color.Error;
        <MudChip Color="@color" Size="Size.Small">@product.Status</MudChip>
    }
)
```

## Authorization

The component integrates with ASP.NET Core authorization. Configure permissions:

```csharp
_context = new EntityServerTableContext<Product, Guid, ProductRequest>(
    // ... other parameters
    searchAction: "View",      // Permission for viewing/searching
    createAction: "Create",    // Permission for creating
    updateAction: "Update",    // Permission for updating
    deleteAction: "Delete",    // Permission for deleting
    exportAction: "Export",    // Permission for exporting
    entityResource: "Products" // Resource name for authorization
);
```

## Custom Actions

Add custom actions to the row menu:

```csharp
<EntityTable TEntity="Product" 
             TId="Guid" 
             TRequest="ProductRequest" 
             Context="@_context"
             EditFormContent="@EditForm"
             ExtraActions="@ExtraActions" />

@code {
    private RenderFragment<Product> ExtraActions => product => __builder =>
    {
        <MudMenuItem Icon="@Icons.Material.Filled.Print"
                     OnClick="@(() => PrintProduct(product))">
            Print Label
        </MudMenuItem>
        <MudMenuItem Icon="@Icons.Material.Filled.History"
                     OnClick="@(() => ViewHistory(product))">
            View History
        </MudMenuItem>
    };
}
```

## Advanced Features

### Conditional Update/Delete

Control which entities can be updated or deleted:

```csharp
_context = new EntityServerTableContext<Order, Guid, OrderRequest>(
    // ... other parameters
    canUpdateEntityFunc: order => order.Status != "Completed",
    canDeleteEntityFunc: order => order.Status == "Draft"
);
```

### Get Details for Edit

Fetch fresh data when editing (instead of using cached list data):

```csharp
_context = new EntityServerTableContext<Product, Guid, ProductRequest>(
    // ... other parameters
    getDetailsFunc: async (id) => await ProductService.GetByIdAsync(id)
);
```

### Entity Duplication

Enable duplicating entities with custom logic:

```csharp
_context = new EntityServerTableContext<Product, Guid, ProductRequest>(
    // ... other parameters
    duplicateFunc: async (product) => 
    {
        var duplicate = await ProductService.GetByIdAsync(product.Id);
        duplicate.Name += " (Copy)";
        return duplicate;
    }
);
```

## Dependencies

The EntityTable component requires:

- **MudBlazor** - UI components
- **Microsoft.AspNetCore.Components.Authorization** - Authorization support
- **Microsoft.AspNetCore.Authorization** - Permission checking

## Notes

- The component uses `IAuthorizationService` for permission checking. If not provided, authorization will be bypassed.
- Export functionality requires implementing the export logic in your API.
- The component handles all modal dialogs internally (Create, Edit, View, Delete confirmations).
- Server-side tables automatically handle pagination, sorting, and filtering through the `PaginationFilter` object.

## Example Models

```csharp
public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
}

public class ProductRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
}
```
