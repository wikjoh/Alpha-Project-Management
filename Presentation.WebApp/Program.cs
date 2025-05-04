using Business.Data;
using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presentation.WebApp.Services;
using Presentation.WebApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));
builder.Services.AddIdentity<UserEntity, IdentityRole>(x =>
{
    x.User.RequireUniqueEmail = true;
    x.Password.RequiredLength = 8;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => !context.Request.Cookies.ContainsKey("cookieConsent");
    options.MinimumSameSitePolicy = SameSiteMode.Lax;
});

builder.Services.ConfigureApplicationCookie(x =>
{
    x.LoginPath = "/auth/signin";
    x.LogoutPath = "/auth/signout";
    x.AccessDeniedPath = "/auth/denied";
    x.Cookie.HttpOnly = true;
    x.Cookie.IsEssential = true;
    x.ExpireTimeSpan = TimeSpan.FromHours(1);
    x.SlidingExpiration = true;
});

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IClientAddressRepository, ClientAddressRepository>();
builder.Services.AddScoped<IMemberProfileRepository, MemberProfileRepository>();
builder.Services.AddScoped<IMemberAddressRepository, MemberAddressRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectMemberRepository, ProjectMemberRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClientAddressService, ClientAddressService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IMemberProfileService, MemberProfileService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IImageUploadService, ImageUploadService>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.AddDefaultRoles(services);
    await DbInitializer.AddDefaultAdmin(services);
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

/* Use UseStaticFiles instead of MapStaticAssets for manual cache control.
 * MapStaticAssets fails caching .svgs etc */
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        const int durationInSeconds = 60 * 60 * 24 * 7; // 1 week
        ctx.Context.Response.Headers.CacheControl = $"public,max-age={durationInSeconds}";
        ctx.Context.Response.Headers.Expires = DateTime.UtcNow.AddDays(7).ToString("R");
    }
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Projects}/{action=Projects}");

app.Run();
