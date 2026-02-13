using FutureV.Data;
using FutureV.Data.Repositories;
using FutureV.Core.Interfaces;
using FutureV.App.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Globalization;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);

var connectionString = ResolveConnectionString(builder.Environment, builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
});

builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<CarInputModelValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddResponseCaching();
builder.Services.AddControllersWithViews();

var culture = new CultureInfo("en-ZA");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    await SeedData.InitializeAsync(context, userManager);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. Change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseResponseCaching();

app.UseAuthentication();
app.UseAuthorization();


app.MapStaticAssets();
app.MapControllerRoute(
    name: "catalog",
    pattern: "catalog/{id:int}",
    defaults: new { controller = "Catalog", action = "Details" });
app.MapControllerRoute(
    name: "discover",
    pattern: "discover",
    defaults: new { controller = "Discover", action = "Index" });
app.MapControllerRoute(
    name: "arriving",
    pattern: "arriving",
    defaults: new { controller = "Arriving", action = "Index" });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

await app.RunAsync();

static string ResolveConnectionString(IHostEnvironment environment, ConfigurationManager configuration)
{
    var configured = configuration.GetConnectionString("DefaultConnection");
    if (!string.IsNullOrWhiteSpace(configured))
    {
        return configured;
    }

    var sqlitePath = Environment.GetEnvironmentVariable("SQLITE_DB_PATH");
    if (!string.IsNullOrWhiteSpace(sqlitePath))
    {
        return BuildSqliteConnectionString(sqlitePath);
    }

    var renderDataDirectory = Environment.GetEnvironmentVariable("RENDER_DATA_DIR");
    if (!string.IsNullOrWhiteSpace(renderDataDirectory))
    {
        var persistentPath = Path.Combine(renderDataDirectory, "futurev.db");
        return BuildSqliteConnectionString(persistentPath);
    }

    var appDataDirectory = Path.Combine(environment.ContentRootPath, "App_Data");
    Directory.CreateDirectory(appDataDirectory);
    var localPath = Path.Combine(appDataDirectory, "futurev.db");
    return BuildSqliteConnectionString(localPath);
}

static string BuildSqliteConnectionString(string path)
{
    var directory = Path.GetDirectoryName(path);
    if (!string.IsNullOrWhiteSpace(directory))
    {
        Directory.CreateDirectory(directory);
    }

    return $"Data Source={path}";
}
