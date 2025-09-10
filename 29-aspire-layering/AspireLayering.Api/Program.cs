using AspireLayering.SharedConfig;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.Configure<SharedMessageOptions>(builder.Configuration.GetSection("SharedMessage"));

var app = builder.Build();

app.MapGet("/config", (IConfiguration config) =>
{
    var root = (IConfigurationRoot)config;
    return Results.Json(new
    {
        ApiInjectedMessage = config["Api:InjectedMessage"],
        ApiExternalApiKey = Mask(config["Api:ExternalApiKey"]),
        SharedMessage = config["SharedMessage:Message"],
        Providers = root.Providers.Select(p => p.ToString())
    });
});

string Mask(string? value) => string.IsNullOrEmpty(value) ? value ?? "" : value.Length < 4 ? "***" : new string('*', value.Length - 4) + value[^4..];

app.Run();
