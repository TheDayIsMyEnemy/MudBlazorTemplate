var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
            providerOptions => providerOptions.EnableRetryOnFailure());
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Stores.MaxLengthForKeys = MaxLengths.StringColumn;
    options.SignIn.RequireConfirmedAccount = true;
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = null;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddUserConfirmation<UserConfirmation>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.LoginPath = new PathString(Constants.LoginPath);
    options.LogoutPath = new PathString(Constants.LogoutPath);
});

//builder.Services.Configure<SecurityStampValidatorOptions>(options =>
//    options.ValidationInterval = TimeSpan.FromSeconds(10));

builder.Services.AddRazorPages(options => options.RootDirectory = "/Features");

builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider<User>>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapBlazorHub()
.RequireAuthorization();

app.MapFallbackToPage("/_Host");

await DbInitializer.ApplyMigrationsAndSeedData(app.Services);

app.Run();