using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
var config = builder.Configuration;

PrintHeader("Individual Keys");
DumpKey("Precedence:Setting1");
DumpKey("Precedence:Setting2");
DumpKey("Precedence:Setting3");
DumpKey("Precedence:SettingFromLaunch");
DumpKey("Precedence:EnvironmentOnly");
DumpKey("Precedence:CommandLineOnly");

PrintHeader("Providers (Top = Highest Precedence)");
int i = 0;
foreach (var src in config.Sources.Reverse())
    Console.WriteLine($"  {++i}. {src.GetType().Name}");

PrintHeader("Configuration Debug View");
Console.WriteLine(config.GetDebugView(ctx => 
    ctx.Key.Contains("Secret", StringComparison.OrdinalIgnoreCase) 
        ? "****" 
        : ctx.Value));

return;

void DumpKey(string key) => Console.WriteLine($"  {key} = '{config[key] ?? "<null>"}'");

void PrintHeader(string title)
{
    Console.WriteLine();
    Console.WriteLine(title);
    Console.WriteLine(new string('-', title.Length));
}
