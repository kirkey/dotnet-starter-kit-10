using System.Net;
using System.Net.Sockets;

namespace FSH.Modules.Ai.Services;

/// <summary>
/// SSRF guard for tenant-supplied URLs (fetch/web-watch/discovery targets). Mirrors the Webhooks
/// module's approach: the handler never follows redirects, and the connect callback screens the
/// resolved IP so DNS rebinding cannot bounce a public hostname to an internal address.
/// </summary>
public static class AiUrlGuard
{
    public static void Validate(Uri uri)
    {
        ArgumentNullException.ThrowIfNull(uri);
        if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
        {
            throw new InvalidOperationException($"Only http(s) URLs may be fetched (got '{uri.Scheme}').");
        }

        if (IsBlockedHost(uri.Host))
        {
            throw new InvalidOperationException($"Fetching '{uri.Host}' is not allowed.");
        }
    }

    public static async ValueTask<Stream> ConnectAsync(SocketsHttpConnectionContext context, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(context);

        var host = context.DnsEndPoint.Host;
        var addresses = await Dns.GetHostAddressesAsync(host, cancellationToken).ConfigureAwait(false);
        var address = addresses.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork)
            ?? addresses.FirstOrDefault()
            ?? throw new InvalidOperationException($"Could not resolve '{host}'.");

        if (IsNonPublic(address))
        {
            throw new InvalidOperationException($"Fetching '{host}' is not allowed.");
        }

        Socket? socket = null;
        try
        {
            socket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            await socket.ConnectAsync(address, context.DnsEndPoint.Port, cancellationToken).ConfigureAwait(false);
            var stream = new NetworkStream(socket, ownsSocket: true);
            socket = null; // ownership transferred to the stream
            return stream;
        }
        finally
        {
            socket?.Dispose();
        }
    }

    private static bool IsBlockedHost(string host) =>
        string.Equals(host, "localhost", StringComparison.OrdinalIgnoreCase)
        || string.Equals(host, "metadata.google.internal", StringComparison.OrdinalIgnoreCase)
        || host.EndsWith(".internal", StringComparison.OrdinalIgnoreCase);

    private static bool IsNonPublic(IPAddress address)
    {
        if (IPAddress.IsLoopback(address))
        {
            return true;
        }

        var bytes = address.GetAddressBytes();
        return address.AddressFamily switch
        {
            AddressFamily.InterNetwork => bytes[0] switch
            {
                10 => true,
                172 => bytes[1] is >= 16 and <= 31,
                192 => bytes[1] == 168,
                169 => bytes[1] == 254,
                127 => true,
                0 => true,
                _ => false
            },
            AddressFamily.InterNetworkV6 => address.IsIPv6LinkLocal
                || address.IsIPv6SiteLocal
                || address.IsIPv6Multicast
                || address.Equals(IPAddress.IPv6Loopback),
            _ => true
        };
    }
}
