using FSH.Playground.Blazor.Api;

namespace FSH.Playground.Blazor.Services.Api;

internal static class ApiClientRegistration
{
    public static IServiceCollection AddApiClients(this IServiceCollection services, IConfiguration configuration)
    {
        string apiBaseUrl = configuration["Api:BaseUrl"]
                            ?? throw new InvalidOperationException("Api:BaseUrl configuration is missing.");

        // Register global error handler
        services.AddScoped<IGlobalErrorHandler, GlobalErrorHandler>();

        // Register named HttpClient with auth handler for API clients
        services.AddHttpClient("ApiClient", (sp, client) =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
        })
        .AddHttpMessageHandler<AuthorizationHeaderHandler>();

        // Register a named HttpClient for token operations (no auth handler to avoid circular dependency)
        services.AddHttpClient("TokenClient", client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
        });

        // Helper to resolve HttpClient with auth handler
        static HttpClient ResolveApiClient(IServiceProvider sp)
        {
            IHttpClientFactory factory = sp.GetRequiredService<IHttpClientFactory>();
            return factory.CreateClient("ApiClient");
        }

        // TokenClient uses the named HttpClient without the AuthorizationHeaderHandler
        // This avoids circular dependency: TokenRefreshService -> ITokenClient -> HttpClient -> AuthorizationHeaderHandler -> TokenRefreshService
        services.AddTransient<ITokenClient>(sp =>
        {
            IHttpClientFactory factory = sp.GetRequiredService<IHttpClientFactory>();
            HttpClient client = factory.CreateClient("TokenClient");
            return new TokenClient(client);
        });

        services.AddTransient<IIdentityClient>(sp =>
            new IdentityClient(ResolveApiClient(sp)));

        services.AddTransient<IAuditsClient>(sp =>
            new AuditsClient(ResolveApiClient(sp)));

        services.AddTransient<ITenantsClient>(sp =>
            new TenantsClient(ResolveApiClient(sp)));

        services.AddTransient<IUsersClient>(sp =>
            new UsersClient(ResolveApiClient(sp)));

        services.AddTransient<ISessionsClient>(sp =>
            new SessionsClient(ResolveApiClient(sp)));

        services.AddTransient<IV1Client>(sp =>
            new V1Client(ResolveApiClient(sp)));

        services.AddTransient<ITodoClient>(sp =>
            new TodoClient(ResolveApiClient(sp)));

        services.AddTransient<ITasksClient>(sp =>
            new TasksClient(ResolveApiClient(sp)));

        services.AddTransient<ITodosClient>(sp =>
            new TodosClient(
                sp.GetRequiredService<IV1Client>(),
                sp.GetRequiredService<ITodoClient>(),
                sp.GetRequiredService<ITasksClient>()));

        services.AddTransient<IHealthClient>(sp =>
            new HealthClient(ResolveApiClient(sp)));

        return services;
    }
}
