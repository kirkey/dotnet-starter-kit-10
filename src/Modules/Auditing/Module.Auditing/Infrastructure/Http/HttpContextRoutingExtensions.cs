using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Auditing.Infrastructure.Http;

internal static class HttpContextRoutingExtensions
{
    public static string? GetRoutePattern(this HttpContext ctx)
        => ctx.GetEndpoint() switch
        {
            RouteEndpoint re => re.RoutePattern.RawText,
            _ => null
        };
}

