using FlavourFlow.Components;
using FlavourFlow.Components;
using FlavourFlow.Components.Account;
using FlavourFlow.Data;
using FlavourFlow.Domains;
using FlavourFlow.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Services
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<FlavourFlowContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<FlavourFlowUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()                 // <--- ADD THIS LINE HERE
    .AddEntityFrameworkStores<FlavourFlowContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<FlavourFlowUser>, IdentityNoOpEmailSender>();

// Custom Services
builder.Services.AddScoped<RecipeService>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<FlavourFlowContext>();

        // This command creates the NEW database and all tables (Recipe, User, Review)
        context.Database.Migrate();

        // This fills it with the sample data
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine("DB Error: " + ex.Message);
    }
}

// 3. Pipeline
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