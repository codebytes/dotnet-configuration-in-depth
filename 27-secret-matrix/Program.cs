using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Configuration.KeyPerFile;
using Microsoft.Extensions.Configuration.Json;

// Secret Strategy Matrix Demo
// Compares how the same logical keys can be supplied via multiple secret/config strategies
// and which one wins by precedence.

var builder = Host.CreateApplicationBuilder(args);

// Add additional providers (KeyPerFile) BEFORE environment variables to show precedence layering.
// Directory 'secrets' (create manually) with files named PlainSecret, ApiKey etc.
builder.Configuration.AddKeyPerFile(directoryPath: "secrets", optional: true);

var config = builder.Configuration;

string[] keys =
[
    "Sample:PlainSecret",   // present in JSON; can override via user secrets / key-per-file / env var / cmd line
    "Sample:ApiKey",        // typically stored in user secrets or env var
    "Sample:ConnString"     // connection string example
];

Console.WriteLine("Secret Strategy Matrix (highest precedence value wins)\n");

PrintHeader();
foreach (var key in keys)
{
    var (value, source) = ResolveWithSource(config, key);
    Console.WriteLine($"{key,-30} | {Truncate(value),-35} | {source}");
}

Console.WriteLine();
PrintProviderOrder(config);

const string instructions = """

Try the following (PowerShell):

1. User Secrets (Development only)
    dotnet user-secrets init --project 22-secret-matrix
    dotnet user-secrets set "Sample:ApiKey" "From User Secrets" --project 22-secret-matrix

2. KeyPerFile (create directory and file with contents)
    mkdir secrets | Out-Null
    Set-Content secrets/PlainSecret "From KeyPerFile file"

3. Environment Variable (overrides previous)
    $env:Sample__PlainSecret = "From Env Var"

4. Command line (highest)
    dotnet run --project 22-secret-matrix -- --Sample:PlainSecret FromCmdLine

5. Inspect precedence after each step
    dotnet run --project 22-secret-matrix --no-build

Cleanup:
    Remove-Item Env:Sample__PlainSecret -ErrorAction SilentlyContinue
""";

Console.WriteLine(instructions);

return;

static (string? value, string source) ResolveWithSource(IConfiguration config, string key)
{
    // Providers are in order of addition (earliest first). Last wins. So we find LAST provider containing the key.
    string? val = null;
    string source = "<none>";
    var root = config as IConfigurationRoot;
    if (root is null) return (null, "<no root>");
    foreach (var p in root.Providers)
    {
        if (p.TryGet(key, out var v))
        {
            val = v;
            source = ProviderName(p);
        }
    }
    return (val, source);
}

static string ProviderName(IConfigurationProvider provider)
{
    return provider.GetType().Name switch
    {
        var n when n.Contains("Json") => provider is not null ? $"Json" : "Json",
        var n when n.Contains("Environment") => "Environment",
        var n when n.Contains("CommandLine") => "CommandLine",
        var n when n.Contains("Secrets") => "UserSecrets",
        var n when n.Contains("KeyPerFile") => "KeyPerFile",
        _ => provider.GetType().Name
    };
}

static void PrintHeader()
{
    Console.WriteLine($"{"Key",-30} | {"Value",-35} | Source");
    Console.WriteLine(new string('-', 30) + "-+-" + new string('-', 35) + "-+-" + new string('-', 25));
}

static void PrintProviderOrder(IConfiguration config)
{
    Console.WriteLine("Provider Order (last has highest precedence):");
    if (config is IConfigurationRoot root)
    {
        int i = 0;
        foreach (var p in root.Providers)
        {
            Console.WriteLine($"  {++i,2}. {ProviderName(p)}");
        }
    }
}

static string Truncate(string? value, int max = 33)
{
    if (value == null) return "<null>";
    return value.Length <= max ? value : value[..(max - 3)] + "...";
}
