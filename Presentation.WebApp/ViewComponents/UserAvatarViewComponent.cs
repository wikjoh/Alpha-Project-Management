using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.ViewComponents;

public class UserAvatarViewComponent(UserManager<UserEntity> userManager, IMemberService memberService) : ViewComponent
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly IMemberService _memberService = memberService;

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var result = await _memberService.GetMemberByIdAsync(_userManager.GetUserId(HttpContext.User)!);
        var memberProfile = result.Data;

        var imageURI = !string.IsNullOrEmpty(memberProfile!.ImageURI)
            ? Url.Content($"~{memberProfile.ImageURI}")
            : Url.Content("~/images/memberDefaultAvatar.svg");

        return View("Default", imageURI);
    }
}
