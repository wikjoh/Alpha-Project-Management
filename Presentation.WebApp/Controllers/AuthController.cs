using Business.Interfaces;
using Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace Presentation.WebApp.Controllers
{
    public class AuthController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;

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
    }
}
