using FSH.CLI.Commands;
using Spectre.Console.Cli;
using System.Reflection;

CommandApp app = new();

app.Configure(config =>
{
    config.SetApplicationName("fsh");
    config.SetApplicationVersion(GetVersion());
    
    // Main command: Create new project
    config.AddCommand<NewCommand>("new")
        .WithDescription("Create a new FullStackHero project")
        .WithExample("new")
        .WithExample("new", "MyApp")
        .WithExample("new", "MyApp", "--preset", "quickstart")
        .WithExample("new", "MyApp", "--type", "api-blazor", "--arch", "monolith", "--db", "postgres");
    
    // Secondary command: Generate code artifacts
    config.AddCommand<GenerateCommand>("generate")
        .WithDescription("Generate code artifacts (modules, features, entities)")
        .WithAlias("gen")
        .WithExample("generate", "module", "Products")
        .WithExample("generate", "feature", "CreateProduct", "--template", "crud")
        .WithExample("generate", "entity", "Product");
    
    // Help command
    config.AddCommand<HelpCommand>("help")
        .WithDescription("Show help and guidance")
        .WithExample("help")
        .WithExample("help", "new")
        .WithExample("help", "templates")
        .WithExample("help", "examples");
});

return await app.RunAsync(args);

static string GetVersion()
{
    Assembly assembly = typeof(Program).Assembly;
    Version? version = assembly.GetName().Version;
    return version?.ToString(3) ?? "1.0.0";
}
