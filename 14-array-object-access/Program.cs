using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
var config = builder.Configuration;

// Manual array/dictionary access by key
Console.WriteLine("=== Manual Key Access ===");
Console.WriteLine($"IPAddressRange:0 = {config["IPAddressRange:0"]}");
Console.WriteLine($"IPAddressRange:1 = {config["IPAddressRange:1"]}");
Console.WriteLine($"IPAddressRange:2 = {config["IPAddressRange:2"]}");
Console.WriteLine($"SupportedVersions:v1 = {config["SupportedVersions:v1"]}");
Console.WriteLine($"SupportedVersions:v3 = {config["SupportedVersions:v3"]}");

// Strongly-typed binding
Console.WriteLine("\n=== Strongly-Typed Binding ===");
var settings = config.Get<AppSettings>() ?? new AppSettings();

Console.WriteLine("IP Addresses:");
foreach (var ip in settings.IPAddressRange)
    Console.WriteLine($"  {ip}");

Console.WriteLine("Supported Versions:");
foreach (var (key, version) in settings.SupportedVersions)
    Console.WriteLine($"  {key} = {version}");

public class AppSettings
{
    public string[] IPAddressRange { get; set; } = [];
    public Dictionary<string, string> SupportedVersions { get; set; } = [];
}