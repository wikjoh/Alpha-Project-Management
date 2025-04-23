using Business.Interfaces;
using Business.Dtos;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models;
using Domain.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.WebApp.Controllers
{
    public class AuthController(IAuthService authService) : Controller
    {
        private readonly IAuthService _authService = authService;

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
    }
}
