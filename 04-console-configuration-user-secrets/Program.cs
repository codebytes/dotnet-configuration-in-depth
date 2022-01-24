using Microsoft.Extensions.Configuration;
using Azure.Identity;
using Azure.Security.KeyVault;
using Azure.Security.KeyVault.Secrets;

IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json")
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>()
    .AddCommandLine(args)
    .Build();

var kvUri = "https://" + configuration["key_vault_name"] + ".vault.azure.net";

var client = new SecretClient(new Uri(kvUri), 
    new ClientSecretCredential(
        configuration["AZURE_TENANT_ID"], 
        configuration["AZURE_CLIENT_ID"], 
        configuration["AZURE_CLIENT_SECRET"]));

var key = await client.GetSecretAsync("myKey");
Console.WriteLine($"Mykey: {key.Value.Value}");

var name = args.Any() ? args[0] : "World";
// See https://aka.ms/new-console-template for more information
Console.WriteLine($"{configuration["greeting"]}, {name}.");
Console.WriteLine($"Environment: {configuration["environment"]}");
