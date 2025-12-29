using FSH.Framework.Shared.Persistence;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FSH.Framework.Persistence;

public sealed class DatabaseOptionsStartupLogger(
    ILogger<DatabaseOptionsStartupLogger> logger,
    IOptions<DatabaseOptions> options)
    : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        DatabaseOptions options1 = options.Value;
        logger.LogInformation("current db provider: {Provider}", options1.Provider);
        logger.LogInformation("for docs: https://www.fullstackhero.net");
        logger.LogInformation("sponsor: https://opencollective.com/fullstackhero");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

