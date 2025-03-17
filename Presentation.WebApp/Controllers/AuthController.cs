using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers
{
    public class AuthController : Controller
    {
        //[Route("CreateAccount")]
        public IActionResult CreateAccount()
        {
            return View();
        }
    }
}
