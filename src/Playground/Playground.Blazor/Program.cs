using FSH.Framework.Blazor.UI;
using FSH.Framework.Blazor.UI.Components.Navigation.Services;
using FSH.Framework.Blazor.UI.Theme;
using FSH.Playground.Blazor.Components;
using FSH.Playground.Blazor.Configuration;
using FSH.Playground.Blazor.Services;
using FSH.Playground.Blazor.Services.Api;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configure HTTP/3 support (only override in production, respect launchSettings in dev)
if (!builder.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(8080, listenOptions =>
        {
            listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2AndHttp3;
        });
    });
}

builder.Services.AddHeroUI();

// Authentication & Authorization
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();

// Cookie Authentication for SSR support
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/auth/logout";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();

// Distributed Cache (required by theme state factory)
builder.Services.AddDistributedMemoryCache();

// Simple cookie-based authentication
builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();

// Tenant theme services
builder.Services.AddScoped<ITenantThemeState, TenantThemeState>(); // For Interactive mode
builder.Services.AddScoped<IThemeStateFactory, CachedThemeStateFactory>(); // For SSR mode

// User profile state for syncing across components
builder.Services.AddScoped<IUserProfileState, UserProfileState>();

// Authorization header handler for API calls
builder.Services.AddScoped<AuthorizationHeaderHandler>();

// Token refresh service for handling expired access tokens
builder.Services.AddScoped<ITokenRefreshService, TokenRefreshService>();

builder.Services.AddApiClients(builder.Configuration);

// Response Compression for static assets and API responses
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProvider>();
    options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProvider>();
});

builder.Services.Configure<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});

builder.Services.Configure<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});

// Output Caching for static responses
builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(policyBuilder => policyBuilder
#pragma warning disable CA1307 // PathString.StartsWithSegments is case-insensitive by design
        .With(c => c.HttpContext.Request.Path.StartsWithSegments("/health"))
#pragma warning restore CA1307
        .Expire(TimeSpan.FromSeconds(10)));
});

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure Blazor Server Circuit Options
builder.Services.AddServerSideBlazor(options =>
{
    options.DetailedErrors = builder.Environment.IsDevelopment();
    options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
    options.DisconnectedCircuitMaxRetained = 100;
    options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(1);
    options.MaxBufferedUnacknowledgedRenderBatches = 10;
});

WebApplication app = builder.Build();

// Initialize menu service
var menuService = app.Services.GetRequiredService<IMenuService>();
menuService.RegisterMenuSections(MenuConfiguration.GetMenuSections());

// Configure exception handler
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

// Simple health endpoints for ALB/ECS
app.MapGet("/health/ready", () => Results.Ok(new { status = "Healthy" }))
   .AllowAnonymous();

app.MapGet("/health/live", () => Results.Ok(new { status = "Alive" }))
   .AllowAnonymous();

app.UseResponseCompression(); // Must come before UseStaticFiles
app.UseOutputCache();
app.UseHttpsRedirection();
app.UseAuthentication(); // Must come before UseAuthorization
app.UseAuthorization();
app.UseAntiforgery();

app.MapSimpleBffAuthEndpoints();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Fallback route for 404 handling
app.MapFallback(async (HttpContext context) =>
{
    context.Response.StatusCode = 404;
    await context.Response.WriteAsync("""
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <base href="/" />
    <title>404 - Page Not Found | FSH Playground</title>
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap" rel="stylesheet" />
    <link href="_content/MudBlazor/MudBlazor.min.css" rel="stylesheet" />
    <link href="_content/FSH.Framework.Blazor.UI/css/fsh-theme.css" rel="stylesheet" />
    <style>
        body { font-family: 'Inter', sans-serif; background: #f5f7fa; margin: 0; padding: 0; }
        .not-found-container { min-height: 100vh; display: flex; align-items: center; justify-content: center; padding: 2rem 1rem; }
        .not-found-content { text-align: center; max-width: 600px; }
        .not-found-icon { font-size: 120px; color: #2563EB; margin-bottom: 2rem; }
        .not-found-title { font-size: 2.5rem; font-weight: 600; margin-bottom: 1rem; color: #1e293b; }
        .not-found-code { font-size: 3rem; font-weight: 700; color: #2563EB; margin-bottom: 1rem; }
        .not-found-description { font-size: 1.1rem; line-height: 1.8; color: #64748b; margin-bottom: 2rem; }
        .btn { display: inline-block; padding: 12px 24px; margin: 0.5rem; border-radius: 4px; text-decoration: none; font-weight: 500; transition: all 0.2s; }
        .btn-primary { background: #2563EB; color: white; }
        .btn-primary:hover { background: #1d4ed8; }
        .btn-outlined { border: 2px solid #2563EB; color: #2563EB; background: transparent; }
        .btn-outlined:hover { background: #eff6ff; }
    </style>
</head>
<body>
    <div class="not-found-container">
        <div class="not-found-content">
            <div class="not-found-icon">🔍</div>
            <h1 class="not-found-title">Page Not Found</h1>
            <div class="not-found-code">404</div>
            <p class="not-found-description">
                The page you're looking for doesn't exist or has been moved.<br />
                Let's get you back on track.
            </p>
            <div>
                <a href="/" class="btn btn-primary">Go to Home</a>
                <a href="javascript:history.back()" class="btn btn-outlined">Go Back</a>
            </div>
        </div>
    </div>
</body>
</html>
""");
});

await app.RunAsync();
