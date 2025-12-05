using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
var configuration = builder.Configuration;

using IHost host = builder.Build();

// Application code should start here.
Console.WriteLine("Hello, World!");
Console.WriteLine($"{configuration["greeting"]}, {configuration["environment"]}!");
await host.RunAsync();
