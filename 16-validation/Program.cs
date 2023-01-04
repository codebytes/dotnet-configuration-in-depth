using Microsoft.Extensions.DependencyInjection.Extensions; 
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<WebHookSettings>()
    .BindConfiguration(WebHookSettings.ConfigurationSectionName)
    .ValidateDataAnnotations()
    .Validate(config =>
        {
            if (config.WebhookUrl.Length < 20)
            {
                return false;
            }

            return true;
        }, "URL length is too short.")
   .Validate(webHookSettings =>
        {
            return webHookSettings.WebhookUrl.StartsWith("https://");
        }, "WebHookUrl must start with https://")
    .ValidateOnStart();

builder.Services.AddSingleton(resolver =>
        resolver.GetRequiredService<IOptions<WebHookSettings>>().Value);

builder.Services.TryAddEnumerable(
    ServiceDescriptor.Singleton
        <IValidateOptions<WebHookSettings>, ValidateWebHookSettingsOptions>());

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// app.MapRazorPages();
app.MapGet("/", (WebHookSettings options) => options);

app.Run();
