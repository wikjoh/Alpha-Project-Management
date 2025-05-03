using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.ViewComponents;

public class UserAvatarViewComponent(UserManager<UserEntity> userManager) : ViewComponent
{
    private readonly UserManager<UserEntity> _userManager = userManager;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await _userManager.GetUserAsync(HttpContext.User);
        var imageURI = Url.Content($"~{user?.MemberProfile?.ImageURI}") ?? Url.Content("~/images/memberDefaultAvatar.svg");

        return View("Default", imageURI);
    }
}
