using Business.Interfaces;
using Business.Dtos;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models;
using Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Data.Entities;
using System.Security.Claims;

namespace Presentation.WebApp.Controllers;

public class AuthController(IAuthService authService, SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager) : Controller
{
    private readonly IAuthService _authService = authService;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly UserManager<UserEntity> _userManager = userManager;

    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(UserSignUpViewModel form)
    {
        if (!ModelState.IsValid)
            return View(form);


        var result = await _authService.SignUpAsync(form.MapTo<UserSignUpForm>(), form.Password);
        switch (result.StatusCode)
        {
            case 201:
                return RedirectToAction("SignIn", "Auth");

            case 400:
                ModelState.AddModelError("", "Required field(s) invalid.");
                return View(form);

            case 409:
                ModelState.AddModelError("Email", "User already exists.");
                return View(form);

            default:
                ModelState.AddModelError("", "An Unexpected Error Occured.");
                return View(form);
        }
    }


    public IActionResult SignIn(string returnUrl = "")
    {
        ViewBag.ReturnUrl = returnUrl;
        ViewBag.ErrorMessage = "";
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return LocalRedirect("~/");
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(UserSignInForm form, string returnUrl = "/Projects/Projects")
    {
        ViewBag.ReturnUrl = returnUrl;
        ViewBag.ErrorMessage = "";




        if (ModelState.IsValid)
        {
            var result = await _authService.LoginAsync(form);
            if (result == null)
            {
                ViewBag.ErrorMessage = "Unable to handle login. Please try later.";
                return View(form);
            }

            if (result.Success)
            {
                if (Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return Redirect("/");
            }
        }

        ViewBag.ErrorMessage = "Incorrect email or password";
        return View(form);
    }


    public IActionResult AdminSignIn(string returnUrl = "")
    {
        ViewBag.ReturnUrl = returnUrl;
        ViewBag.ErrorMessage = "";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AdminSignIn(AdminSignInViewModel vm, string returnUrl = "/Projects/Projects")
    {
        ViewBag.ReturnUrl = returnUrl;
        ViewBag.ErrorMessage = "";

        if (ModelState.IsValid)
        {
            var form = vm.MapTo<AdminSignInForm>();
            var result = await _authService.AdminLoginAsync(form);
            if (result == null)
            {
                ViewBag.ErrorMessage = "Unable to handle login. Please try later.";
                return View(form);
            }

            if (result.Success)
            {
                if (Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return Redirect("/");
            }
        }

        ViewBag.ErrorMessage = "Incorrect email or password";
        return View(vm);
    }


    [HttpPost]
    public IActionResult ExternalSignIn(string provider, string returnUrl = null!)
    {
        if (string.IsNullOrEmpty(provider))
        {
            ModelState.AddModelError("", "Invalid provider");
            return View("SignIn");
        }

        var redirectUrl = Url.Action("ExternalSignInCallBack", "Auth", new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    public async Task<IActionResult> ExternalSignInCallback(string returnUrl = null!, string remoteError = null!)
    {
        returnUrl ??= Url.Content("~/");

        if (!string.IsNullOrEmpty(remoteError))
        {
            ModelState.AddModelError("", $"Error from external provider: {remoteError}");
            return View("SignIn");
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
            return RedirectToAction("SignIn");

        var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        if (signInResult.Succeeded)
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            string firstName = "";
            string lastName = "";

            try
            {
                firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName)!;
                lastName = info.Principal.FindFirstValue(ClaimTypes.Surname)!;
            }
            catch { }

            string email = info.Principal.FindFirstValue(ClaimTypes.Email)!;
            string username = $"ext_{info.LoginProvider.ToLower()}_{email}";

            var user = new UserEntity { UserName = username, Email = email, FirstName = firstName, LastName = lastName };

            var identityResult = await _userManager.CreateAsync(user);
            if (identityResult.Succeeded)
            {
                await _userManager.AddLoginAsync(user, info);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }

            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View("SignIn");
        }
    }

    [HttpPost]
    public IActionResult AdminExternalSignIn(string provider, string returnUrl = null!)
    {
        if (string.IsNullOrEmpty(provider))
        {
            ModelState.AddModelError("", "Invalid provider");
            return View("AdminSignIn");
        }

        var redirectUrl = Url.Action("AdminExternalSignInCallback", "Auth", new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    public async Task<IActionResult> AdminExternalSignInCallback(string returnUrl = null!, string remoteError = null!)
    {
        returnUrl ??= Url.Content("~/");

        if (!string.IsNullOrEmpty(remoteError))
        {
            ModelState.AddModelError("", $"Error from external provider: {remoteError}");
            return View("AdminSignIn");
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
            return RedirectToAction("AdminSignIn");

        var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
        if (signInResult.Succeeded)
        {
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                await _signInManager.SignOutAsync();
                ModelState.AddModelError("", "User not authorized as admin.");
                return View("AdminSignIn");
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (!isAdmin)
            {
                await _signInManager.SignOutAsync();
                ModelState.AddModelError("", "You do not have admin rights.");
                return View("AdminSignIn");
            }

            return LocalRedirect(returnUrl);
        }

        ModelState.AddModelError("", "Admin account creation using external login is not allowed.");
        return View("AdminSignIn");
    }
}
