using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Helpers;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;
using Silicin_MVC.Services;
using System.Security.Claims;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddDefaultIdentity<UserEntity>(x =>
{
    x.User.RequireUniqueEmail = true;
    x.SignIn.RequireConfirmedAccount = false;
    x.Password.RequiredLength = 8;
}).AddRoles<IdentityRole>()
.AddEntityFrameworkStores<DataContext>();




builder.Services.AddScoped<AddressRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<FeatureItemRepository>();
builder.Services.AddScoped<FeatureRepository>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<FeatureService>();
builder.Services.AddScoped<AddressManager>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<CourseService>();
//builder.Services.AddScoped<AccountManager>();
builder.Services.AddScoped<CategoryIdAssigner>();


builder.Services.ConfigureApplicationCookie(x =>
{

    x.LoginPath = "/signin";
    x.LogoutPath = "/signout";
    x.AccessDeniedPath = "/denied";

    x.Cookie.HttpOnly = true;
    x.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    x.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    x.SlidingExpiration = true;
});


builder.Services.AddAuthorization(x =>
{
    x.AddPolicy("SuperAdmins", policy => policy.RequireRole("SuperAdmin"));
    x.AddPolicy("CIO", policy => policy.RequireRole("SuperAdmin", "CIO"));
    x.AddPolicy("Admins", policy => policy.RequireRole("SuperAdmin", "CIO", "Admin"));
    x.AddPolicy("Managers", policy => policy.RequireRole("SuperAdmin", "CIO", "Admin", "Manager"));
});
// GIT HUB AVVISADE DESSA SECRET COD OCH EFTER DET JAG KUNDE INTE PUSHA NÅT MER
builder.Services.AddAuthentication().AddFacebook(x =>
{
    x.AppId = "8031866396892885";
    x.AppSecret = "64a511c49cc224f48cf03eeee77c71b4";
    x.Fields.Add("first_name");
    x.Fields.Add("last_name");

});

builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "515206983626-2384c9tkm839fv5o8eig9bl4q0oc4sqm.apps.googleusercontent.com";
    options.ClientSecret = "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";
    options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
    options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
    options.SaveTokens = true;
});


var app = builder.Build();

app.UseHsts();
app.UseStatusCodePagesWithReExecute("/error", "?statusCode={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = ["SuperAdmin", "Admin", "CIO", "Manager", "User"];
    foreach (string role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }

    }

}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

