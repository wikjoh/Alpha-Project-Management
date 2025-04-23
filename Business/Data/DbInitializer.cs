using Business.Dtos;
using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Business.Data;

public static class DbInitializer
{
    public static async Task AddDefaultRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        string[] defaultRoles = ["User", "Admin"];

        foreach (var role in defaultRoles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    public static async Task AddDefaultAdmin(IServiceProvider serviceProvider)
    {
        var memberService = serviceProvider.GetRequiredService<IMemberService>();
        var userManager = serviceProvider.GetRequiredService<UserManager<UserEntity>>();
        var defaultAdmin = new AddMemberForm {
            UserForm = new UserSignUpForm { Email = "hans@poweruser.ec", FirstName = "Hans", LastName = "Poweruser" },
            JobTitle = "CTO",
            MemberAddress = new MemberAddressForm { StreetAddress = "Testroad 00", PostalCode = "12345", City = "TestCity"}
        };

        var exists = await userManager.Users.AnyAsync(x => x.Email == defaultAdmin.UserForm.Email);
        if (!exists)
        {
            await memberService.AddMemberAsync(defaultAdmin);
            var user = await userManager.FindByEmailAsync(defaultAdmin.UserForm.Email);
            await userManager.AddPasswordAsync(user!, "P@ssw0rd");
        }
    }
}
