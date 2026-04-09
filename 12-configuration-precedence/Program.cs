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
var index = 0;
foreach (var source in config.Sources.Reverse())
    Console.WriteLine($"  {++index}. {source.GetType().Name}");

PrintHeader("Configuration Debug View (Precedence only)");
var debugView = config.GetDebugView(ctx =>
    ctx.Key.Contains("Secret", StringComparison.OrdinalIgnoreCase) ? "****" : ctx.Value);

// Filter to only show Precedence keys
foreach (var line in debugView.Split(Environment.NewLine))
{
    if (line.Contains("Precedence", StringComparison.OrdinalIgnoreCase))
        Console.WriteLine(line);
}

return;

void DumpKey(string key) => Console.WriteLine($"  {key} = '{config[key] ?? "<null>"}'");

void PrintHeader(string title)
{
    Console.WriteLine();
    Console.WriteLine(title);
    Console.WriteLine(new string('-', title.Length));
}
