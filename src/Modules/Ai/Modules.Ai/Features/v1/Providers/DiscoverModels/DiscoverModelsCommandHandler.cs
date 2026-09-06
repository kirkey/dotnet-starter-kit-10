using System.Net.Http.Json;
using System.Text.Json.Serialization;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Providers;
using Mediator;

namespace FSH.Modules.Ai.Features.v1.Providers.DiscoverModels;

/// <summary>
/// Draft-first model discovery: queries the provider's model catalog with a caller-supplied
/// endpoint + key and returns candidates. Writes nothing — no provider, model, or secret is
/// persisted until an explicit save. The draft key lives only in this request.
/// </summary>
public sealed class DiscoverModelsCommandHandler(IHttpClientFactory httpFactory)
    : ICommandHandler<DiscoverModelsCommand, IReadOnlyList<DiscoveredModelDto>>
{
    public async ValueTask<IReadOnlyList<DiscoveredModelDto>> Handle(DiscoverModelsCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var client = httpFactory.CreateClient("AiProviders");
        using var request = new HttpRequestMessage(
            HttpMethod.Get, $"{command.BaseUrl.Trim().TrimEnd('/')}/models");
        if (!string.IsNullOrWhiteSpace(command.ApiKey))
        {
            request.Headers.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", command.ApiKey.Trim());
        }

        using var response = await client.SendAsync(request, cancellationToken).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new FSH.Framework.Core.Exceptions.CustomException(
                $"Model discovery against '{command.BaseUrl}' failed with status {(int)response.StatusCode}.",
                errors: null,
                System.Net.HttpStatusCode.BadGateway);
        }

        var payload = await response.Content
            .ReadFromJsonAsync<ModelListResponse>(cancellationToken)
            .ConfigureAwait(false);

        return (payload?.Data ?? [])
            .Where(m => !string.IsNullOrWhiteSpace(m.Id))
            .Select(m => new DiscoveredModelDto(m.Id.Trim()))
            .Distinct()
            .ToList();
    }

    private sealed record ModelListResponse(
        [property: JsonPropertyName("data")] IReadOnlyList<ModelEntry> Data);

    private sealed record ModelEntry(
        [property: JsonPropertyName("id")] string Id);
}
