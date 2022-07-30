using Microsoft.Extensions.Configuration;
using Spectre.Console;

var mappings = new Dictionary<string, string>
{
    { "--greeting", "greeting:message" },
    { "--color", "greeting:color" },
    { "--env", "environment" },
};
IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(optional: true)
    .AddCommandLine(args, mappings)
    .Build();
    
var name = "World";
// See https://aka.ms/new-console-template for more information
AnsiConsole.MarkupLine($"[{configuration["greeting:color"]}]{configuration["greeting:message"]}, {name}![/]");
Console.WriteLine($"Configuration: {configuration["environment"]}");

