using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Azure.Identity;
using Azure.Security.KeyVault;
using Azure.Security.KeyVault.Secrets;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .AddCommandLine(args)
    .AddAzureKeyVault(new Uri("https://cayers-dotnet-kv.vault.azure.net/"), new DefaultAzureCredential())
    .Build();
    
Console.WriteLine($"Message: {configuration["Message"]}");

var name = args.Any() ? args[0] : "World";
// See https://aka.ms/new-console-template for more information
Console.WriteLine($"{configuration["greeting"]}, {name}.");
Console.WriteLine($"Environment: {configuration["environment"]}");
