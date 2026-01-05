using Asp.Versioning;
using Asp.Versioning.Builder;
using FSH.Framework.Persistence;
using FSH.Framework.Shared.Identity;
using FSH.Framework.Web.Modules;
using FSH.Module.Store.Data;
using FSH.Module.Store.Features.v1.POS.CreatePOS;
using FSH.Module.Store.Features.v1.POS.GetPOSByStore;
using FSH.Module.Store.Features.v1.Stores.CreateStore;
using FSH.Module.Store.Features.v1.Stores.GetStores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace FSH.Module.Store;

/// <summary>
/// Store Module - Management system for convenience stores.
/// 
/// **Purpose:**
/// Provides a complete feature set for managing physical store locations
/// and Point of Sale (POS) terminals for small to medium-sized convenience stores.
/// 
/// **Key Features:**
/// - Store location management
/// - POS terminal management
/// - Multi-tenant isolation
/// - Full audit trail
/// 
/// **Architecture:**
/// - Uses Entity Framework Core with PostgreSQL/MSSQL support
/// - Implements Domain-Driven Design
/// - Supports multi-tenancy through Finbuckle
/// - Uses CQRS pattern with Mediator library
/// 
/// **Entities:**
/// - Store: Physical store locations
/// - PointOfSale: POS terminals in stores
/// </summary>
public class StoreModule : IModule
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        // Register permissions
        PermissionConstants.Register(StorePermissionConstants.GetPermissions());
        
        // Register DbContext with multi-tenancy support
        builder.Services.AddHeroDbContext<StoreDbContext>();
        
        // Register health checks
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<StoreDbContext>(
                name: "db:store",
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
            .MapGroup("api/v{version:apiVersion}/store")
            .WithTags("Store")
            .WithApiVersionSet(apiVersionSet);
        
        // Map Store endpoints
        group.MapCreateStoreEndpoint();
        group.MapGetStoresEndpoint();
        
        // Map POS endpoints
        group.MapCreatePOSEndpoint();
        group.MapGetPOSByStoreEndpoint();
    }
}
