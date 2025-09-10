var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<FileOptions>("pdf",
    builder.Configuration.GetSection("MarkdownConverter:pdf"));
builder.Services.Configure<FileOptions>("doc",
    builder.Configuration.GetSection("MarkdownConverter:doc"));
builder.Services.Configure<FileOptions>("html",
    builder.Configuration.GetSection("MarkdownConverter:html"));
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

app.MapRazorPages();

app.Run();
