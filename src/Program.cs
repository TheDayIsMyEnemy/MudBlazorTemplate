using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using MudBlazorTemplate;
using MudBlazorTemplate.Areas.Identity;
using MudBlazorTemplate.Data;
using MudBlazorTemplate.Data.Entities;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

#if DEBUG
builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    options
        .UseSqlServer(config.GetConnectionString("Default"))
        .EnableSensitiveDataLogging();
});
#else
builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    options.UseSqlServer(config.GetConnectionString("Default"));
});
#endif

builder.Services.AddDefaultIdentity<User>(options =>
{
    options.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "MudBlazorTemplate";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = Constants.LoginPath;
    options.LogoutPath = Constants.LogoutPath;
    options.SlidingExpiration = true;
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

builder.Services.AddScoped<AuthenticationStateProvider, 
    RevalidatingIdentityAuthenticationStateProvider>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); ;
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

await DbInitializer.ApplyMigrationsAndSeedData(app.Services);

app.Run();