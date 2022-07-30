using Microsoft.Extensions.Configuration;
using Spectre.Console;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(optional: true)
    .AddCommandLine(args)
    .Build();

var name = "World";

//dotnet run --greeting:color="blue"

AnsiConsole.MarkupLine($"[{configuration["greeting:color"]}]{configuration["greeting:message"]}, {name}![/]");
Console.WriteLine($"Configuration: {configuration["environment"]}");
