using Asp.Versioning;
using FSH.Framework.Eventing;
using FSH.Framework.Persistence;
using FSH.Framework.Shared.Constants;
using FSH.Framework.Web.HttpResilience;
using FSH.Framework.Web.Modules;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Data;
using FSH.Modules.Ai.Features.v1.Sources.AddWebSource;
using FSH.Modules.Ai.Features.v1.Sources.DeleteSource;
using FSH.Modules.Ai.Features.v1.Sources.ListSources;
using FSH.Modules.Ai.Features.v1.Sources.RefreshWebSource;
using FSH.Modules.Ai.Features.v1.Providers.CreateProvider;
using FSH.Modules.Ai.Features.v1.Providers.DeleteProvider;
using FSH.Modules.Ai.Features.v1.Providers.DiscoverModels;
using FSH.Modules.Ai.Features.v1.Providers.GetProvider;
using FSH.Modules.Ai.Features.v1.Providers.ListProviders;
using FSH.Modules.Ai.Features.v1.Providers.SetProviderDefault;
using FSH.Modules.Ai.Features.v1.Providers.SetProviderSecret;
using FSH.Modules.Ai.Features.v1.Providers.UpdateProvider;
using FSH.Modules.Ai.Features.v1.Chat.CreateChatSession;
using FSH.Modules.Ai.Features.v1.Chat.DeleteChatSession;
using FSH.Modules.Ai.Features.v1.Chat.GetChatSession;
using FSH.Modules.Ai.Features.v1.Chat.ListChatSessions;
using FSH.Modules.Ai.Features.v1.Chat.SendChatMessage;
using FSH.Modules.Ai.Features.v1.Departments.CreateDepartment;
using FSH.Modules.Ai.Features.v1.Departments.DeleteDepartment;
using FSH.Modules.Ai.Features.v1.Departments.ListDepartments;
using FSH.Modules.Ai.Features.v1.Departments.UpdateDepartment;
using FSH.Modules.Ai.Features.v1.Detection.GetLocalAgents;
using FSH.Modules.Ai.Features.v1.Runs.EndAgentRun;
using FSH.Modules.Ai.Features.v1.Runs.GetAgentRun;
using FSH.Modules.Ai.Features.v1.Runs.ListAgentRuns;
using FSH.Modules.Ai.Features.v1.Runs.StartAgentRun;
using FSH.Modules.Ai.Features.v1.Runtimes.ListRuntimes;
using FSH.Modules.Ai.Features.v1.Runtimes.RefreshRuntimeCatalog;
using FSH.Modules.Ai.Features.v1.Agents.ArchiveAgent;
using FSH.Modules.Ai.Features.v1.Agents.CreateAgent;
using FSH.Modules.Ai.Features.v1.Agents.CreateAgentCopy;
using FSH.Modules.Ai.Features.v1.Agents.GetAgent;
using FSH.Modules.Ai.Features.v1.Agents.ListAgents;
using FSH.Modules.Ai.Features.v1.Agents.RestoreAgent;
using FSH.Modules.Ai.Features.v1.Agents.UpdateAgent;
using FSH.Modules.Ai.Features.v1.Schedules.CreateSchedule;
using FSH.Modules.Ai.Features.v1.Schedules.DeleteSchedule;
using FSH.Modules.Ai.Features.v1.Schedules.GetSchedule;
using FSH.Modules.Ai.Features.v1.Schedules.ListSchedules;
using FSH.Modules.Ai.Features.v1.Schedules.SetScheduleWebhook;
using FSH.Modules.Ai.Features.v1.Schedules.UpdateSchedule;
using FSH.Modules.Ai.Features.v1.Schedules.StartScheduledRun;
using FSH.Modules.Ai.Features.v1.Sources.UpdateSourceChunks;
using FSH.Modules.Ai.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

[assembly: FshModule(typeof(FSH.Modules.Ai.AiModule), 850)]

namespace FSH.Modules.Ai;

public sealed class AiModule : IModule
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        PermissionConstants.Register(AiPermissions.All);

        builder.Services.AddHeroDbContext<AiDbContext>();
        builder.Services.AddScoped<IDbInitializer, AiDbInitializer>();
        builder.Services.AddIntegrationEventHandlers(typeof(AiModule).Assembly);

        // Tenant-supplied fetch targets: never follow redirects; screen resolved IPs at connect.
        builder.Services.AddHttpClient("AiWebFetch")
            .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
            {
                AllowAutoRedirect = false,
                ConnectCallback = AiUrlGuard.ConnectAsync,
            })
            .AddHeroResilience(builder.Configuration);

        // Operator-configured provider endpoints (trusted): resilient, redirects allowed.
        builder.Services.AddHttpClient("AiProviders")
            .AddHeroResilience(builder.Configuration);

        builder.Services.AddScoped<IHtmlToMarkdownConverter, HtmlToMarkdownConverter>();
        builder.Services.AddScoped<IWebPageFetcher, WebPageFetcher>();
        builder.Services.AddScoped<IAiSecretProtector, AiSecretProtector>();
        builder.Services.AddScoped<IAiProviderSelector, AiProviderSelector>();
        builder.Services.AddScoped<ILocalAgentDetector, LocalAgentDetector>();
        builder.Services.AddScoped<RuntimeCatalogService>();
        builder.Services.AddScoped<IChatRunner, ChatRunner>();
        builder.Services.AddScoped<Jobs.WebSourceFetchJob>();
        builder.Services.AddScoped<Jobs.ChunkAndEmbedJob>();
        builder.Services.AddScoped<Jobs.AgentRunJob>();
        builder.Services.AddScoped<Jobs.AgentScheduleJob>();
        builder.Services.AddScoped<IAiTextChunker, AiTextChunker>();
        builder.Services.Configure<AiOptions>(builder.Configuration.GetSection("Ai"));

        builder.Services.AddHealthChecks()
            .AddDbContextCheck<AiDbContext>(
                name: "db:ai",
                failureStatus: HealthStatus.Unhealthy);
    }

    public void ConfigureMiddleware(IApplicationBuilder app)
    {
        // No custom middleware needed
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        var versionSet = endpoints.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        var group = endpoints
            .MapGroup("api/v{version:apiVersion}/ai")
            .WithTags("Ai")
            .WithApiVersionSet(versionSet)
            .RequireAuthorization();

        // Feature endpoints land here in tasks 2.x–4.x.
        group.MapAddWebSourceEndpoint();
        group.MapRefreshWebSourceEndpoint();
        group.MapListSourcesEndpoint();
        group.MapUpdateSourceChunksEndpoint();
        group.MapDeleteSourceEndpoint();
        group.MapCreateProviderEndpoint();
        group.MapUpdateProviderEndpoint();
        group.MapDeleteProviderEndpoint();
        group.MapDiscoverModelsEndpoint();
        group.MapGetProviderEndpoint();
        group.MapListProvidersEndpoint();
        group.MapSetProviderSecretEndpoint();
        group.MapSetProviderDefaultEndpoint();
        group.MapCreateChatSessionEndpoint();
        group.MapListChatSessionsEndpoint();
        group.MapGetChatSessionEndpoint();
        group.MapDeleteChatSessionEndpoint();
        group.MapSendChatMessageEndpoint();
        group.MapSendChatMessageStreamEndpoint();
        group.MapGetLocalAgentsEndpoint();
        group.MapCreateDepartmentEndpoint();
        group.MapUpdateDepartmentEndpoint();
        group.MapDeleteDepartmentEndpoint();
        group.MapListDepartmentsEndpoint();
        group.MapCreateAgentEndpoint();
        group.MapUpdateAgentEndpoint();
        group.MapGetAgentEndpoint();
        group.MapListAgentsEndpoint();
        group.MapArchiveAgentEndpoint();
        group.MapRestoreAgentEndpoint();
        group.MapCreateAgentCopyEndpoint();
        group.MapStartAgentRunEndpoint();
        group.MapListAgentRunsEndpoint();
        group.MapGetAgentRunEndpoint();
        group.MapEndAgentRunEndpoint();
        group.MapCreateScheduleEndpoint();
        group.MapUpdateScheduleEndpoint();
        group.MapDeleteScheduleEndpoint();
        group.MapListSchedulesEndpoint();
        group.MapGetScheduleEndpoint();
        group.MapSetScheduleWebhookEndpoint();
        group.MapStartScheduledRunEndpoint();
        group.MapListRuntimesEndpoint();
        group.MapRefreshRuntimeCatalogEndpoint();
    }
}
