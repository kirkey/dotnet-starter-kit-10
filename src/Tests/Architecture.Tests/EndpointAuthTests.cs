using Shouldly;
using Xunit;

namespace Architecture.Tests;

/// <summary>
/// Every Minimal API endpoint must be auth-classified: permission-gated via
/// <c>.RequirePermission()</c>, authorized for self-service/ownership-scoped access
/// (<c>.RequireAuthorization()</c> / <c>[Authorize]</c>), or explicitly anonymous.
/// Locks the audit in <c>openspec/changes/api-best-practices-alignment/audit-endpoints.md</c>:
/// adding a new anonymous endpoint requires allowlisting it here with a reason,
/// so no endpoint ever ships ungated by accident (see GetMyInvoices, fixed 2026-09).
/// </summary>
public class EndpointAuthTests
{
    // Anonymous-by-design endpoints: file name (without extension) + reason.
    private static readonly (string File, string Reason)[] AnonymousByDesign =
    [
        ("GenerateTokenEndpoint", "login must be reachable pre-auth"),
        ("RefreshTokenEndpoint", "refresh by definition"),
        ("ConfirmEmailEndpoint", "email link flow"),
        ("ForgotPasswordEndpoint", "password-reset initiation"),
        ("ResetPasswordEndpoint", "password-reset completion"),
        ("SelfRegisterUserEndpoint", "public signup"),
    ];

    [Fact]
    public void Endpoints_Should_Be_Gated_Or_Allowlisted()
    {
        string modulesRoot = Path.Combine(GetSolutionRoot(), "src", "Modules");

        string[] endpointFiles = Directory
            .GetFiles(modulesRoot, "*Endpoint.cs", SearchOption.AllDirectories)
            .Where(path => !IsBuildOutput(path))
            .ToArray();

        endpointFiles.Length.ShouldBeGreaterThan(0,
            "No *Endpoint.cs files found; the auth classification sweep would be a no-op.");

        var violations = new List<string>();

        foreach (string path in endpointFiles)
        {
            string code = StripLineComments(File.ReadAllText(path));
            string file = Path.GetFileNameWithoutExtension(path);

            bool gated = code.Contains(".RequirePermission(", StringComparison.Ordinal);
            bool authorized = code.Contains(".RequireAuthorization(", StringComparison.Ordinal)
                || code.Contains("[Authorize]", StringComparison.Ordinal);
            bool anonymous = code.Contains("AllowAnonymous", StringComparison.Ordinal);

            if (!gated && !authorized && !anonymous)
            {
                violations.Add(
                    $"{file} has no auth gate (.RequirePermission/.RequireAuthorization/[Authorize]/AllowAnonymous)");
                continue;
            }

            if (anonymous && !AnonymousByDesign.Any(a =>
                    string.Equals(a.File, file, StringComparison.Ordinal)))
            {
                violations.Add(
                    $"{file} is anonymous but not on the AnonymousByDesign allowlist — " +
                    "classify it or gate it");
            }
        }

        violations.ShouldBeEmpty(
            "Unauthenticated or unclassified endpoints found. " +
            $"Violations: {string.Join("; ", violations)}");
    }

    private static bool IsBuildOutput(string path) =>
        path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.Ordinal) ||
        path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}", StringComparison.Ordinal);

    private static string StripLineComments(string code) =>
        string.Join('\n', code.Split('\n').Select(line =>
        {
            int index = line.IndexOf("//", StringComparison.Ordinal);
            return index < 0 ? line : line[..index];
        }));

    private static string GetSolutionRoot()
    {
        // Same root-finding as ModuleArchitectureTests: walk up until `src` exists.
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (directory is not null && !Directory.Exists(Path.Combine(directory.FullName, "src")))
        {
            directory = directory.Parent;
        }

        if (directory is null)
        {
            throw new InvalidOperationException("Unable to locate solution root containing 'src' folder.");
        }

        return directory.FullName;
    }
}
