using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// Demonstrates configuration source ordering & precedence including launchSettings.json (Dev only),
// appsettings & environment specific files, environment variables, and command line.

var builder = Host.CreateApplicationBuilder(args);

// Add explicit additional JSON to highlight ordering (appsettings*.json already added by default template in Host.CreateApplicationBuilder)
// (Left here for clarity; not strictly required unless customizing order.)
// builder.Configuration.Sources already contains: appsettings.json, appsettings.{Environment}.json (optional), user secrets (if dev), env vars, command line.

var config = builder.Configuration;
// Build a temp logger factory (simplest) - since we only need logging for boot diagnostics.
builder.Logging.AddSimpleConsole();
using var loggerFactory = LoggerFactory.Create(lb => lb.AddSimpleConsole());
var logger = loggerFactory.CreateLogger("ConfigPrecedence");

PrintHeader("Individual Keys");
DumpKey("Precedence:Setting1");
DumpKey("Precedence:Setting2");
DumpKey("Precedence:Setting3");
DumpKey("Precedence:SettingFromLaunch");
DumpKey("Precedence:EnvironmentOnly");
DumpKey("Precedence:CommandLineOnly");

PrintHeader("Providers (Top = Highest Precedence in Resolution Order)");
int i = 0;
foreach (var src in config.Sources.Reverse())
{
    logger.LogInformation("{Index,2}. {Type}", ++i, src.GetType().Name);
}

PrintHeader("Configuration Debug View");
Console.WriteLine(config.GetDebugView());

PrintInstructions();

return; // No host run needed; this is an inspection utility.

void DumpKey(string key)
{
    Console.WriteLine($"{key} = '{config[key] ?? "<null>"}'");
}

void PrintHeader(string title)
{
    Console.WriteLine();
    Console.WriteLine(new string('=', title.Length));
    Console.WriteLine(title);
    Console.WriteLine(new string('=', title.Length));
}

void PrintInstructions()
{
    Console.WriteLine(@"
Try the following to observe precedence (PowerShell examples):

1. Baseline (appsettings + appsettings.Development + launchSettings.json overrides):
   dotnet run --project 19-config-precedence --no-build

2. Add environment variable (takes precedence over JSON files but below command line):
   $env:Precedence__Setting2='FromEnvVar'
   dotnet run --project 19-config-precedence --no-build

3. Command line override of Setting2:
   dotnet run --project 19-config-precedence -- --Precedence:Setting2 FromCmd

4. Command line provide a key only defined there:
   dotnet run --project 19-config-precedence -- --Precedence:CommandLineOnly OnlyCmd

5. Observe launchSettings influence: the key Precedence:SettingFromLaunch is only set via launchSettings.json when using 'dotnet run' (Development profile).

Use --environment Production to see absence of appsettings.Development.json & launchSettings influence:
   dotnet run --project 19-config-precedence -- --environment Production

Clear environment variable after test:
   Remove-Item Env:Precedence__Setting2 -ErrorAction SilentlyContinue
");
}
