using FSH.CLI.Commands;
using Spectre.Console.Cli;
using System.Reflection;

CommandApp app = new();

app.Configure(config =>
{
    config.SetApplicationName("fsh");
    config.SetApplicationVersion(GetVersion());

    config.AddCommand<NewCommand>("new")
        .WithDescription("Create a new FullStackHero project")
        .WithExample("new")
        .WithExample("new", "MyApp")
        .WithExample("new", "MyApp", "--preset", "quickstart")
        .WithExample("new", "MyApp", "--type", "api-blazor", "--arch", "monolith", "--db", "postgres");
});

return await app.RunAsync(args);

static string GetVersion()
{
    Assembly assembly = typeof(Program).Assembly;
    Version? version = assembly.GetName().Version;
    return version?.ToString(3) ?? "1.0.0";
}
