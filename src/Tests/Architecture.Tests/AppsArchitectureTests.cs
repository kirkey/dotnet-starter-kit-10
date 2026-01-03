using NetArchTest.Rules;
using Shouldly;
using Xunit;

namespace Architecture.Tests;

public class AppsArchitectureTests
{
    [Fact]
    public void Modules_Should_Not_Depend_On_Apps_Hosts()
    {
        // Assemblies / namespaces that represent Apps hosts.
        string[] appsNamespaces =
        {
            "FSH.Basic.Api",
            "Bsic.Blazor"
        };

        TestResult? result = Types
            .InCurrentDomain()
            .That()
            .ResideInNamespace("FSH.Modules")
            .Should()
            .NotHaveDependencyOnAny(appsNamespaces)
            .GetResult();

        IReadOnlyList<string> failingTypes = result.FailingTypeNames ?? Array.Empty<string>();

        result.IsSuccessful.ShouldBeTrue(
            "Module code must not depend on Apps host assemblies. " +
            $"Failing types: {string.Join(", ", failingTypes)}");
    }

    [Fact]
    public void Apps_Hosts_Should_Not_Depend_On_Module_Internals()
    {
        // Hosts may depend on module contracts and module root types,
        // but should not directly reference feature or data-layer namespaces.
        string[] forbiddenNamespaces =
        {
            "FSH.Modules.Auditing.Features",
            "FSH.Modules.Auditing.Data",
            "FSH.Modules.Identity.Features",
            "FSH.Modules.Identity.Data",
            "FSH.Modules.Multitenancy.Features",
            "FSH.Modules.Multitenancy.Data"
        };

        TestResult? hostResult = Types
            .InCurrentDomain()
            .That()
            .ResideInNamespace("FSH.Apps")
            .Or()
            .ResideInNamespace("Bsic.Blazor")
            .Should()
            .NotHaveDependencyOnAny(forbiddenNamespaces)
            .GetResult();

        IReadOnlyList<string> hostFailingTypes = hostResult.FailingTypeNames ?? Array.Empty<string>();

        hostResult.IsSuccessful.ShouldBeTrue(
            "Apps hosts should not depend directly on module feature or data internals. " +
            $"Failing types: {string.Join(", ", hostFailingTypes)}");
    }
}

internal static class ModuleArchitectureTestsFixture
{
    public static readonly string SolutionRoot = GetSolutionRoot();

    private static string GetSolutionRoot()
    {
        DirectoryInfo? directory = new(Directory.GetCurrentDirectory());

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
