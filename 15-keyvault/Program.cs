using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Azure.Identity;
using Azure.Security.KeyVault;
using Azure.Security.KeyVault.Secrets;

var builder = WebApplication.CreateBuilder(args);
var keyVaultName = builder.Configuration["KeyVaultName"] ?? throw new ArgumentNullException("KeyVaultName");
var keyVaultUri = $"https://{keyVaultName}.vault.azure.net/";

//Connect to your KeyVault using the URI
builder.Configuration
    .AddAzureKeyVault(new Uri(keyVaultUri), new DefaultAzureCredential());
var app = builder.Build();
var configuration = app.Configuration;

Console.WriteLine($"Message: {configuration["Message"]}");

var name = args.Any() ? args[0] : "World";
// See https://aka.ms/new-console-template for more information
Console.WriteLine($"{configuration["greeting"]}, {name}.");
Console.WriteLine($"Environment: {configuration["environment"]}");
