using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Data;

public static class DbInitializer
{
    public static async Task AddDefaultRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        string[] defaultRoles = { "User", "Admin" };

        foreach (var role in defaultRoles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    //public static async Task AddDefaultUser(IServiceProvider serviceProvider)
    //{

    //}
}
