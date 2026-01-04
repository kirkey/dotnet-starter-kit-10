// Add this hosted service class once in your auditing module

using FSH.Module.Auditing.Contracts;
using Microsoft.Extensions.Hosting;

namespace FSH.Module.Auditing.Core;

public sealed class AuditingConfigurator(
    IAuditPublisher publisher,
    IAuditSerializer serializer,
    IEnumerable<IAuditEnricher> enrichers)
    : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Audit.Configure(publisher, serializer, enrichers);
        return Task.CompletedTask;
    }
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

