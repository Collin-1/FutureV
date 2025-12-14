using FutureV.Data;
using FutureV.Data.Repositories;
using FutureV.Core.Interfaces;
using FutureV.App.Validators;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using Npgsql;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => options.LowercaseUrls = true);

var connectionString = ResolveConnectionString(builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

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

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

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

static string ResolveConnectionString(ConfigurationManager configuration)
{
    var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
    if (!string.IsNullOrWhiteSpace(databaseUrl))
    {
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':');
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.IsDefaultPort ? 5432 : uri.Port,
            Username = userInfo[0],
            Password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : string.Empty,
            Database = uri.AbsolutePath.Trim('/'),
            SslMode = SslMode.Require
        };

        if (!string.IsNullOrEmpty(uri.Query))
        {
            var parameters = QueryHelpers.ParseQuery(uri.Query);
            foreach (var parameter in parameters)
            {
                var value = parameter.Value.LastOrDefault();
                if (!string.IsNullOrEmpty(value))
                {
                    builder[parameter.Key] = value;
                }
            }
        }

        return builder.ConnectionString;
    }

    var connection = configuration.GetConnectionString("DefaultConnection");
    if (!string.IsNullOrWhiteSpace(connection))
    {
        return connection;
    }

    throw new InvalidOperationException("A PostgreSQL connection string was not provided. Set ConnectionStrings__DefaultConnection or DATABASE_URL.");
}
