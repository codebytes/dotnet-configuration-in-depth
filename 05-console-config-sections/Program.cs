using Microsoft.Extensions.Configuration;
using Spectre.Console;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(optional: true)
    .AddCommandLine(args)
    .Build();

var name = args.Any() ? args[0] : "World";
// See https://aka.ms/new-console-template for more information
AnsiConsole.MarkupLine($"[{configuration["greeting:color"]}]{configuration["greeting:message"]}, {name}![/]");
Console.WriteLine($"Configuration: {configuration["environment"]}");
