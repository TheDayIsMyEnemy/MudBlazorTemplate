using MudBlazorTemplate.Areas.Identity;
using MudBlazorTemplate.Data;
using MudBlazorTemplate.Data.Entities;
using MudBlazorTemplate.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

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

builder.Services.AddDefaultIdentity<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

builder.Services.AddTransient<UserManager<User>>();

builder.Services.AddScoped<AuthenticationStateProvider,
    RevalidatingIdentityAuthenticationStateProvider<User>>();

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

await DatabaseInitializer.SeedData(app.Services);

app.Run();