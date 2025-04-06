using Business.Interfaces;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers
{
    public class AuthController(IUserService userService, IAuthService authService) : Controller
    {
        private readonly IUserService _userService = userService;
        private readonly IAuthService _authService = authService;

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserSignUpForm form)
        {
            if (!ModelState.IsValid)
                return View(form);

            var result = await _userService.CreateAsync(form);
            switch (result.StatusCode)
            {
                case 201:
                    return RedirectToAction("SignIn", "Auth");

                case 400:
                    ModelState.AddModelError("Invalid Fields", "Required field(s) not valid.");
                    return View(form);

                case 409:
                    ModelState.AddModelError("Exists", "User already exists.");
                    return View(form);

                default:
                    ModelState.AddModelError("Unexpected Error", "An Unexpected Error Occured.");
                    return View(form);
            }
        }


        public IActionResult SignIn(string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ErrorMessage = "";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInForm form, string returnUrl = "/")
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.LoginAsync(form);
                if (result.Success)
                {
                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Projects", "Projects");
                }
            }

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ErrorMessage = "Incorrect email or password";
            return View(form);
        }
    }
}
