using Microsoft.Extensions.Configuration;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

var name = "World";
// See https://aka.ms/new-console-template for more information
Console.WriteLine($"{configuration["greeting"]}, {name}.");
Console.WriteLine($"Configuration: {configuration["environment"]}");

