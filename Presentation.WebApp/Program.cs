using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
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
    pattern: "{controller=Auth}/{action=CreateAccount}/{id?}");


app.Run();
