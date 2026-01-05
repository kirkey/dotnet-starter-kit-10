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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace FSH.Module.Catalog;

/// <summary>
/// Catalog Module - A comprehensive module for managing product catalog.
/// 
/// **Purpose:**
/// Provides a complete feature set for managing product categories, brands, and products.
/// 
/// **Key Features:**
/// - Hierarchical category management
/// - Brand management with website links
/// - Product management with pricing, inventory, and specifications
/// - Multi-tenant isolation for catalog data
/// - Full audit trail for compliance and tracking
/// 
/// **Architecture:**
/// - Uses Entity Framework Core with PostgreSQL/MSSQL support
/// - Implements Domain-Driven Design
/// - Supports multi-tenancy through Finbuckle
/// - Uses CQRS pattern with Mediator library
/// - Includes FluentValidation for request validation
/// 
/// **Entities:**
/// - Category: Product categories with hierarchical support
/// - Brand: Product brands/manufacturers
/// - Product: Products with pricing, inventory, and relationships
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
            .WithTags("Catalog")
            .WithApiVersionSet(apiVersionSet);
        
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
