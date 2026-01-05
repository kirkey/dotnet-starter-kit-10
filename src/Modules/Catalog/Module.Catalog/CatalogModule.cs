using Asp.Versioning;
using Asp.Versioning.Builder;
using FSH.Framework.Persistence;
using FSH.Framework.Shared.Identity;
using FSH.Framework.Web.Modules;
using FSH.Module.Catalog.Data;
using FSH.Module.Catalog.Features.v1.Brands.CreateBrand;
using FSH.Module.Catalog.Features.v1.Brands.GetBrands;
using FSH.Module.Catalog.Features.v1.Categories.CreateCategory;
using FSH.Module.Catalog.Features.v1.Categories.GetCategories;
using FSH.Module.Catalog.Features.v1.Products.CreateProduct;
using FSH.Module.Catalog.Features.v1.Products.GetProducts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace FSH.Module.Catalog;

/// <summary>
/// Catalog Module - A comprehensive module for managing products, categories, and brands.
/// 
/// **Purpose:**
/// Provides a complete product catalog system with support for:
/// - Hierarchical category structure
/// - Brand management
/// - Product management with inventory tracking
/// - SKU and barcode support
/// - Multi-tenancy support
/// - Full audit trail for compliance and tracking
/// 
/// **Key Features:**
/// - CRUD operations for categories, brands, and products
/// - Hierarchical category organization (parent-child relationships)
/// - Product pricing and cost tracking
/// - Inventory management (quantity in stock, reorder levels)
/// - Product status management (Draft, Active, OutOfStock, Discontinued)
/// - Multi-tenancy isolation
/// 
/// **Architecture:**
/// - Uses Entity Framework Core with PostgreSQL/MSSQL support
/// - Implements Domain-Driven Design with aggregate pattern
/// - Supports multi-tenancy through Finbuckle
/// - Uses CQRS pattern with Mediator library for commands/queries
/// - Includes FluentValidation for request validation
/// 
/// **Entities:**
/// - Category: Hierarchical category organization
/// - Brand: Product manufacturers/brands
/// - Product: Main product entity with pricing and inventory
/// 
/// **Permissions:**
/// - Categories: View, Search, Create, Update, Delete
/// - Brands: View, Search, Create, Update, Delete
/// - Products: View, Search, Create, Update, Delete
/// </summary>
public class CatalogModule : IModule
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        // Register permissions
        PermissionConstants.Register(CatalogPermissionConstants.GetPermissions());
        
        // Register DbContext with multi-tenancy support
        builder.Services.AddHeroDbContext<CatalogDbContext>();
        
        // Register health checks
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<CatalogDbContext>(
                name: "db:catalog",
                failureStatus: HealthStatus.Unhealthy);
    }
    
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        // API versioning setup
        ApiVersionSet apiVersionSet = endpoints.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();
        
        // Route group configuration
        RouteGroupBuilder group = endpoints
            .MapGroup("api/v{version:apiVersion}/catalog")
            .WithApiVersionSet(apiVersionSet)
            .WithOpenApi();
        
        group.WithGroupName("Catalog");
        
        // Map Category endpoints
        group.MapCreateCategoryEndpoint();
        group.MapGetCategoriesEndpoint();
        
        // Map Brand endpoints
        group.MapCreateBrandEndpoint();
        group.MapGetBrandsEndpoint();
        
        // Map Product endpoints
        group.MapCreateProductEndpoint();
        group.MapGetProductsEndpoint();
    }
}
