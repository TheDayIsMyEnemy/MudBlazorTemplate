using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using MudBlazorTemplate.Data;

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

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();