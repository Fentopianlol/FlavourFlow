using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FlavourFlow.Components;
using FlavourFlow.Components.Account;
using FlavourFlow.Data;
using FlavourFlow.Domains;
using FlavourFlow.Services; // Add this namespace for your services

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// 2. Database Connection
// We get the string first so we can use it below
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Register FlavourFlowContext (NOT ApplicationDbContext)
builder.Services.AddDbContext<FlavourFlowContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// 3. Authentication & Authorization Setup
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

// 4. Identity Configuration
// We link Identity to FlavourFlowContext here
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

builder.Services.AddIdentityCore<FlavourFlowUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<FlavourFlowContext>() // <--- Changed from ApplicationDbContext
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<FlavourFlowUser>, IdentityNoOpEmailSender>();

// 5. Register Your Custom Services
// These allow you to inject @inject RecipeService into your razor pages
builder.Services.AddScoped<RecipeService>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

// === START SEEDER BLOCK ===
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<FlavourFlow.Data.FlavourFlowContext>();
        FlavourFlow.Data.DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}
// === END SEEDER BLOCK ===

// 6. Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapAdditionalIdentityEndpoints();

app.Run();