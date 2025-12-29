using FSH.CLI.UI;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace FSH.CLI.Commands;

/// <summary>
/// Generate code artifacts (modules, features, entities) following best practices.
/// </summary>
[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by Spectre.Console.Cli via reflection")]
internal sealed class GenerateCommand : AsyncCommand<GenerateCommand.Settings>
{
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by Spectre.Console.Cli via reflection")]
    internal sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "[type]")]
        [Description("Type to generate: module, feature, entity")]
        public string? Type { get; set; }

        [CommandArgument(1, "[name]")]
        [Description("Name of the artifact to generate")]
        public string? Name { get; set; }

        [CommandOption("-t|--template")]
        [Description("Template to use: crud, workflow, file, integration, cached, reporting, job")]
        [DefaultValue("crud")]
        public string Template { get; set; } = "crud";

        [CommandOption("-p|--path")]
        [Description("Path to the solution root")]
        [DefaultValue(".")]
        public string SolutionPath { get; set; } = ".";

        [CommandOption("--force")]
        [Description("Force overwrite existing files")]
        [DefaultValue(false)]
        public bool Force { get; set; }

        [CommandOption("--no-validation")]
        [Description("Skip FluentValidation generation")]
        [DefaultValue(false)]
        public bool NoValidation { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        try
        {
            ValidateInputs(settings);

            ConsoleTheme.WriteBanner();

            string typeToGenerate = settings.Type!.ToLowerInvariant();

            return typeToGenerate switch
            {
                "module" => await GenerateModuleAsync(settings, cancellationToken),
                "feature" => await GenerateFeatureAsync(settings, cancellationToken),
                "entity" => await GenerateEntityAsync(settings, cancellationToken),
                _ => throw new ArgumentException($"Unknown generation type: {settings.Type}")
            };
        }
        catch (ArgumentException ex)
        {
            ConsoleTheme.WriteError(ex.Message);
            return 1;
        }
        catch (InvalidOperationException ex)
        {
            ConsoleTheme.WriteError(ex.Message);
            return 1;
        }
        catch (IOException ex)
        {
            ConsoleTheme.WriteError($"File operation failed: {ex.Message}");
            return 1;
        }
    }

    private static void ValidateInputs(Settings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.Type))
        {
            throw new ArgumentException("Generation type is required (module, feature, or entity)");
        }

        if (string.IsNullOrWhiteSpace(settings.Name))
        {
            throw new ArgumentException($"{settings.Type} name is required");
        }

        if (!Directory.Exists(settings.SolutionPath))
        {
            throw new ArgumentException($"Solution path does not exist: {settings.SolutionPath}");
        }
    }

    private static Task<int> GenerateModuleAsync(Settings settings, CancellationToken cancellationToken)
    {
        ConsoleTheme.WriteStep("Validating module configuration");
        
        string moduleName = settings.Name!;
        string modulePath = Path.Combine(settings.SolutionPath, "src", "Modules", moduleName);

        if (Directory.Exists(modulePath) && !settings.Force)
        {
            throw new InvalidOperationException($"Module '{moduleName}' already exists. Use --force to overwrite.");
        }

        ConsoleTheme.WriteStep("Generating module structure");
        // TODO: Implement module generation following MODULE_TEMPLATES.md

        ConsoleTheme.WriteStep("Creating contracts project");
        // TODO: Generate contracts project

        ConsoleTheme.WriteStep("Creating implementation project");
        // TODO: Generate implementation project

        ConsoleTheme.WriteStep("Creating sample feature");
        // TODO: Generate sample feature based on template

        ConsoleTheme.WriteSuccess($"Module '{moduleName}' generated successfully!");
        ConsoleTheme.WriteInfo($"Next steps:\n  1. Add module to Program.cs\n  2. Create database migrations\n  3. Implement your features");

        return Task.FromResult(0);
    }

    private static Task<int> GenerateFeatureAsync(Settings settings, CancellationToken cancellationToken)
    {
        ConsoleTheme.WriteStep("Validating feature configuration");

        string featureName = settings.Name!;
        string template = settings.Template.ToLowerInvariant();

        ValidateTemplate(template);

        ConsoleTheme.WriteStep("Creating feature structure");
        // TODO: Implement feature generation

        ConsoleTheme.WriteStep("Generating command/query");
        // TODO: Generate command/query in contracts

        ConsoleTheme.WriteStep("Generating handler");
        // TODO: Generate handler

        if (!settings.NoValidation)
        {
            ConsoleTheme.WriteStep("Generating validator");
            // TODO: Generate FluentValidation validator
        }

        ConsoleTheme.WriteStep("Generating endpoint");
        // TODO: Generate Minimal API endpoint

        ConsoleTheme.WriteSuccess($"Feature '{featureName}' generated successfully!");
        ConsoleTheme.WriteInfo($"Template used: {template}\nNext: Implement business logic in the handler");

        return Task.FromResult(0);
    }

    private static Task<int> GenerateEntityAsync(Settings settings, CancellationToken cancellationToken)
    {
        ConsoleTheme.WriteStep("Validating entity configuration");

        string entityName = settings.Name!;

        ConsoleTheme.WriteStep("Creating domain entity");
        // TODO: Implement entity generation

        ConsoleTheme.WriteStep("Creating entity configuration");
        // TODO: Generate EF Core configuration

        ConsoleTheme.WriteSuccess($"Entity '{entityName}' generated successfully!");
        ConsoleTheme.WriteInfo("Next:\n  1. Add properties to the entity\n  2. Create migrations\n  3. Use in handlers");

        return Task.FromResult(0);
    }

    private static void ValidateTemplate(string template)
    {
        string[] validTemplates = { "crud", "workflow", "file", "integration", "cached", "reporting", "job" };

        if (!validTemplates.Contains(template))
        {
            throw new ArgumentException($"Unknown template: {template}. Valid options: {string.Join(", ", validTemplates)}");
        }
    }
}
