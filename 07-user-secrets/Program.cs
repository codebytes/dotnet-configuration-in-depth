using Microsoft.Extensions.Configuration;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .AddCommandLine(args)
    .Build();

var name = "World";
// See https://aka.ms/new-console-template for more information
Console.WriteLine($"{configuration["greeting"]}, {name}.");
Console.WriteLine($"Environment: {configuration["environment"]}");
