using FSH.CLI.UI;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace FSH.CLI.Commands;

[SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by Spectre.Console.Cli via reflection")]
internal sealed class HelpCommand : AsyncCommand<HelpCommand.Settings>
{
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Instantiated by Spectre.Console.Cli via reflection")]
    internal sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "[topic]")]
        [Description("Help topic: new, generate, templates, patterns, examples")]
        [DefaultValue(null)]
        public string? Topic { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        ConsoleTheme.WriteBanner();

        string topic = settings.Topic?.ToLowerInvariant() ?? "general";

        switch (topic)
        {
            case "new":
                ShowNewHelp();
                break;

            case "generate":
                ShowGenerateHelp();
                break;

            case "templates":
                ShowTemplatesHelp();
                break;

            case "patterns":
                ShowPatternsHelp();
                break;

            case "examples":
                ShowExamplesHelp();
                break;

            default:
                ShowGeneralHelp();
                break;
        }

        return await Task.FromResult(0);
    }

    private static void ShowGeneralHelp()
    {
        AnsiConsole.MarkupLine("[bold]FullStackHero CLI - Quick Help[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold yellow]COMMANDS:[/]");
        AnsiConsole.MarkupLine("  [cyan]fsh new[/]        Create a new FullStackHero project");
        AnsiConsole.MarkupLine("  [cyan]fsh generate[/]   Generate code artifacts");
        AnsiConsole.MarkupLine("  [cyan]fsh help[/]       Show help");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold yellow]HELP TOPICS:[/]");
        AnsiConsole.MarkupLine("  [green]new templates patterns examples[/]");
    }

    private static void ShowNewHelp()
    {
        AnsiConsole.MarkupLine("[bold]fsh new - Create Projects[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[cyan]fsh new MyApp --preset quickstart[/]");
        AnsiConsole.MarkupLine("[cyan]fsh new MyApp --type api-blazor --arch monolith[/]");
    }

    private static void ShowGenerateHelp()
    {
        AnsiConsole.MarkupLine("[bold]fsh generate - Code Generation[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[cyan]fsh generate module Products[/]");
        AnsiConsole.MarkupLine("[cyan]fsh generate feature GetProducts[/]");
    }

    private static void ShowTemplatesHelp()
    {
        AnsiConsole.MarkupLine("[bold]Templates[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("CRUD | Workflow | File | Integration | Cached | Reporting | Job");
    }

    private static void ShowPatternsHelp()
    {
        AnsiConsole.MarkupLine("[bold]Patterns[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("Read: COPILOT_INSTRUCTIONS.md in your project");
    }

    private static void ShowExamplesHelp()
    {
        AnsiConsole.MarkupLine("[bold]Examples[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("fsh new ecommerce --preset production");
        AnsiConsole.MarkupLine("fsh generate module Products");
    }
}
