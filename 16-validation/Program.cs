using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions<WebHookSettings>()
    .BindConfiguration("WebHook")
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddSingleton(resolver => 
        resolver.GetRequiredService<IOptions<WebHookSettings>>().Value);


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
