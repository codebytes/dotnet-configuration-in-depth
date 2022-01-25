var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration["ConnectionStrings:AppConfig"];
builder.Host.ConfigureAppConfiguration(builder =>
                {
                    //Connect to your App Config Store using the connection string
                    builder.AddAzureAppConfiguration(options =>
                    {
                        options.Connect(connectionString)
                            .ConfigureRefresh(refresh =>
                                    {
                                        refresh.Register("TestApp:Settings:Sentinel", refreshAll: true)
                                            .SetCacheExpiration(new TimeSpan(0, 0, 5));
                                    });
                    });
                });
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
