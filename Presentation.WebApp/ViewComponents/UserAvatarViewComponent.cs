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
        string imageURI;

        if (memberProfile == null || string.IsNullOrEmpty(memberProfile?.ImageURI))
            imageURI = Url.Content("~/images/memberDefaultAvatar.svg");
        else
            imageURI = Url.Content($"~{memberProfile.ImageURI}");

        return View("Default", imageURI);
    }
}
