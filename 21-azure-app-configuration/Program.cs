using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration["ConnectionStrings:AppConfig"];

//Connect to your App Config Store using the connection string
builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(connectionString)
        .ConfigureRefresh(refresh =>
                {
                    refresh.Register("TestApp:Settings:Sentinel", refreshAll: true)
                        .SetRefreshInterval(new TimeSpan(0, 0, 5));
                });
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAzureAppConfiguration();
builder.Services.Configure<Settings>(builder.Configuration.GetSection("TestApp:Settings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAzureAppConfiguration();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
