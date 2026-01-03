using FSH.Framework.Web;
using FSH.Framework.Web.Modules;
using FSH.Modules.Auditing;
using FSH.Modules.Identity;
using FSH.Modules.Multitenancy;
using FSH.Modules.Todos;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
{
    static void Require(IConfiguration config, string key)
    {
        if (string.IsNullOrWhiteSpace(config[key]))
        {
            throw new InvalidOperationException($"Missing required configuration '{key}' in Production.");
        }
    }

    ConfigurationManager config = builder.Configuration;
    Require(config, "DatabaseOptions:ConnectionString");
    Require(config, "CachingOptions:Redis");
    Require(config, "JwtOptions:SigningKey");
}

// Define module assemblies - these will be used for module registration
Assembly[] moduleAssemblies =
[
    typeof(IdentityModule).Assembly,
    typeof(MultitenancyModule).Assembly,
    typeof(AuditingModule).Assembly,
    typeof(TodoModule).Assembly
];

// Register Mediator with automatic assembly scanning
// Using marker types from each module and contract assembly
builder.Services.AddMediator(o =>
{
    o.ServiceLifetime = ServiceLifetime.Scoped;
    o.Assemblies =
    [
        typeof(IdentityModule),
        typeof(MultitenancyModule),
        typeof(AuditingModule),
        typeof(TodoModule),
        typeof(FSH.Modules.Identity.Contracts.Services.IUserService),
        typeof(FSH.Modules.Multitenancy.Contracts.ITenantService),
        typeof(FSH.Modules.Auditing.Contracts.AuditEnvelope),
        typeof(FSH.Modules.Todos.Contracts.v1.Todos.GetTodosQuery)
    ];
});

builder.AddHeroPlatform(o =>
{
    o.EnableCaching = true;
    o.EnableMailing = true;
    o.EnableJobs = true;
});

builder.AddModules(moduleAssemblies);
WebApplication app = builder.Build();

app.UseHeroMultiTenantDatabases();
app.UseHeroPlatform(p =>
{
    p.MapModules = true;
    p.ServeStaticFiles = true;
});

// Health check endpoint - can be used for container/load balancer health checks
app.MapGet("/", () => Results.Ok(new 
    { 
        message = "FSH Framework API is running", 
        version = "v1",
        timestamp = DateTimeOffset.UtcNow 
    }))
   .WithName("Root")
   .WithTags("Health")
   .AllowAnonymous()
   .CacheOutput(policy => policy.Expire(TimeSpan.FromMinutes(1)));
await app.RunAsync();
