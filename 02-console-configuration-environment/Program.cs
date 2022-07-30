using Microsoft.Extensions.Configuration;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .AddEnvironmentVariables()
    .Build();

var name = "World";

// powershell
// $env:environment="qa"        
// dotnet run

// bash
// environment=qa dotnet run

// See https://aka.ms/new-console-template for more information
Console.WriteLine($"{configuration["greeting"]}, {name}.");
Console.WriteLine($"Configuration: {configuration["environment"]}");
