using FSH.CLI.Models;
using FSH.CLI.UI;
using FSH.CLI.Validation;
using Spectre.Console;
using System.Reflection;

namespace FSH.CLI.Prompts;

internal static class ProjectWizard
{
    public static ProjectOptions Run(string? initialName = null, string? initialVersion = null)
    {
        ConsoleTheme.WriteBanner();

        // Step 1: Choose preset or custom
        string startChoice = PromptStartChoice();

        if (startChoice != "Custom")
        {
            Preset preset = Presets.All.First(p => p.Name == startChoice);
            string presetName = PromptProjectName(initialName);
            string presetPath = PromptOutputPath();
            string? presetVersion = PromptFrameworkVersion(initialVersion);

            ProjectOptions presetOptions = preset.ToProjectOptions(presetName, presetPath);
            presetOptions.FrameworkVersion = presetVersion;

            ShowSummary(presetOptions);
            return presetOptions;
        }

        // Custom flow
        string name = PromptProjectName(initialName);
        ProjectType type = PromptProjectType();
        ArchitectureStyle architecture = PromptArchitecture(type);
        DatabaseProvider database = PromptDatabase(architecture);
        List<string> features = PromptFeatures(architecture);
        string outputPath = PromptOutputPath();
        string? frameworkVersion = PromptFrameworkVersion(initialVersion);

        ProjectOptions options = new()
        {
            Name = name,
            Type = type,
            Architecture = architecture,
            Database = database,
            InitializeGit = features.Contains("Git Repository"),
            IncludeDocker = features.Contains("Docker Compose"),
            IncludeAspire = features.Contains("Aspire AppHost"),
            IncludeSampleModule = features.Contains("Sample Module (Catalog)"),
            IncludeTerraform = features.Contains("Terraform (AWS)"),
            IncludeGitHubActions = features.Contains("GitHub Actions CI"),
            OutputPath = outputPath,
            FrameworkVersion = frameworkVersion
        };

        ShowSummary(options);
        return options;
    }

    private static string PromptStartChoice()
    {
        List<string> choices = new() { "Custom" };
        choices.AddRange(Presets.All.Select(p => p.Name));

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[dim]Select template[/]")
                .PageSize(10)
                .HighlightStyle(ConsoleTheme.PrimaryStyle)
                .AddChoices(choices)
                .UseConverter(c =>
                {
                    if (c == "Custom")
                        return "Custom [dim]- configure manually[/]";

                    Preset preset = Presets.All.First(p => p.Name == c);
                    return $"{preset.Name} [dim]- {preset.Description}[/]";
                }));

        return choice;
    }

    private static string PromptProjectName(string? initialName)
    {
        if (!string.IsNullOrWhiteSpace(initialName) && OptionValidator.IsValidProjectName(initialName))
        {
            return initialName;
        }

        return AnsiConsole.Prompt(
            new TextPrompt<string>("[dim]Project name:[/]")
                .PromptStyle(ConsoleTheme.PrimaryStyle)
                .ValidationErrorMessage("[red]Invalid name[/]")
                .Validate(name =>
                {
                    if (string.IsNullOrWhiteSpace(name))
                        return Spectre.Console.ValidationResult.Error("Required");

                    if (!char.IsLetter(name[0]))
                        return Spectre.Console.ValidationResult.Error("Must start with a letter");

                    if (!name.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '-' || c == '.'))
                        return Spectre.Console.ValidationResult.Error("Only letters, numbers, _, -, or .");

                    return Spectre.Console.ValidationResult.Success();
                }));
    }

    private static ProjectType PromptProjectType()
    {
        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[dim]Project type[/]")
                .HighlightStyle(ConsoleTheme.PrimaryStyle)
                .AddChoices("API", "API + Blazor"));

        return choice == "API" ? ProjectType.Api : ProjectType.ApiBlazor;
    }

    private static ArchitectureStyle PromptArchitecture(ProjectType projectType)
    {
        List<string> choices = new()
        {
            "Monolith",
            "Microservices"
        };

        // Serverless not available with Blazor
        if (projectType == ProjectType.Api)
        {
            choices.Add("Serverless");
        }

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[dim]Architecture[/]")
                .HighlightStyle(ConsoleTheme.PrimaryStyle)
                .AddChoices(choices));

        return choice switch
        {
            "Monolith" => ArchitectureStyle.Monolith,
            "Microservices" => ArchitectureStyle.Microservices,
            "Serverless" => ArchitectureStyle.Serverless,
            _ => ArchitectureStyle.Monolith
        };
    }

    private static DatabaseProvider PromptDatabase(ArchitectureStyle architecture)
    {
        List<string> choices = new()
        {
            "PostgreSQL",
            "SQL Server"
        };

        // SQLite not available with Microservices
        if (architecture != ArchitectureStyle.Microservices)
        {
            choices.Add("SQLite");
        }

        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[dim]Database[/]")
                .HighlightStyle(ConsoleTheme.PrimaryStyle)
                .AddChoices(choices));

        return choice switch
        {
            "PostgreSQL" => DatabaseProvider.PostgreSQL,
            "SQL Server" => DatabaseProvider.SqlServer,
            "SQLite" => DatabaseProvider.SQLite,
            _ => DatabaseProvider.PostgreSQL
        };
    }

    private static List<string> PromptFeatures(ArchitectureStyle architecture)
    {
        List<string> choices = new()
        {
            "Git Repository",
            "Docker Compose",
            "Sample Module (Catalog)",
            "Terraform (AWS)",
            "GitHub Actions CI"
        };

        // Aspire not available with Serverless
        if (architecture != ArchitectureStyle.Serverless)
        {
            choices.Insert(2, "Aspire AppHost");
        }

        List<string> defaults = new() { "Git Repository", "Docker Compose" };
        if (architecture != ArchitectureStyle.Serverless)
        {
            defaults.Add("Aspire AppHost");
        }

        MultiSelectionPrompt<string> prompt = new MultiSelectionPrompt<string>()
            .Title("[dim]Features[/] [dim italic](space to toggle)[/]")
            .HighlightStyle(ConsoleTheme.PrimaryStyle)
            .InstructionsText("")
            .AddChoices(choices);

        foreach (string item in defaults)
        {
            prompt.Select(item);
        }

        return AnsiConsole.Prompt(prompt);
    }

    private static string PromptOutputPath()
    {
        bool useCurrentDir = AnsiConsole.Confirm("[dim]Create in current directory?[/]", true);

        if (useCurrentDir)
        {
            return ".";
        }

        return AnsiConsole.Prompt(
            new TextPrompt<string>("[dim]Output path:[/]")
                .PromptStyle(ConsoleTheme.PrimaryStyle)
                .DefaultValue(".")
                .ValidationErrorMessage("[red]Invalid path[/]")
                .Validate(path =>
                {
                    if (string.IsNullOrWhiteSpace(path))
                        return Spectre.Console.ValidationResult.Error("Required");

                    return Spectre.Console.ValidationResult.Success();
                }));
    }

    private static string? PromptFrameworkVersion(string? initialVersion)
    {
        // If a version was provided via CLI, use it
        if (!string.IsNullOrWhiteSpace(initialVersion))
        {
            return initialVersion;
        }

        string defaultVersion = GetDefaultFrameworkVersion();

        bool useDefault = AnsiConsole.Confirm(
            $"[dim]Use default FSH version[/] [cyan]{defaultVersion}[/][dim]?[/]",
            true);

        if (useDefault)
        {
            return null; // null means use CLI's version
        }

        return AnsiConsole.Prompt(
            new TextPrompt<string>("[dim]FSH version:[/]")
                .PromptStyle(ConsoleTheme.PrimaryStyle)
                .DefaultValue(defaultVersion)
                .ValidationErrorMessage("[red]Invalid version[/]")
                .Validate(version =>
                {
                    if (string.IsNullOrWhiteSpace(version))
                        return Spectre.Console.ValidationResult.Error("Required");

                    // Basic semver validation
                    if (!System.Text.RegularExpressions.Regex.IsMatch(version, @"^\d+\.\d+\.\d+(-[\w\d\.]+)?$"))
                        return Spectre.Console.ValidationResult.Error("Use semver format (e.g., 10.0.0)");

                    return Spectre.Console.ValidationResult.Success();
                }));
    }

    private static string GetDefaultFrameworkVersion()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        string version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                         ?? assembly.GetName().Version?.ToString()
                         ?? "10.0.0";

        // Remove any +buildmetadata suffix
        int plusIndex = version.IndexOf('+', StringComparison.Ordinal);
        return plusIndex > 0 ? version[..plusIndex] : version;
    }

    private static void ShowSummary(ProjectOptions options)
    {
        ConsoleTheme.WriteHeader("Configuration");

        ConsoleTheme.WriteKeyValue("Name", options.Name, highlight: true);
        ConsoleTheme.WriteKeyValue("Type", FormatEnum(options.Type));
        ConsoleTheme.WriteKeyValue("Architecture", FormatEnum(options.Architecture));
        ConsoleTheme.WriteKeyValue("Database", FormatEnum(options.Database));
        ConsoleTheme.WriteKeyValue("Version", options.FrameworkVersion ?? GetDefaultFrameworkVersion());
        ConsoleTheme.WriteKeyValue("Output", options.OutputPath);

        // Build features list
        List<string> features = new();
        if (options.InitializeGit) features.Add("Git");
        if (options.IncludeDocker) features.Add("Docker");
        if (options.IncludeAspire) features.Add("Aspire");
        if (options.IncludeSampleModule) features.Add("Sample");
        if (options.IncludeTerraform) features.Add("Terraform");
        if (options.IncludeGitHubActions) features.Add("CI");

        ConsoleTheme.WriteKeyValue("Features", features.Count > 0 ? string.Join(", ", features) : "none");

        AnsiConsole.WriteLine();

        if (!AnsiConsole.Confirm("Create project?", true))
        {
            AnsiConsole.MarkupLine("[dim]Cancelled.[/]");
            Environment.Exit(0);
        }
    }

    private static string FormatEnum<T>(T value) where T : Enum =>
        value.ToString() switch
        {
            "Api" => "API",
            "ApiBlazor" => "API + Blazor",
            "PostgreSQL" => "PostgreSQL",
            "SqlServer" => "SQL Server",
            "SQLite" => "SQLite",
            _ => value.ToString()
        };
}
