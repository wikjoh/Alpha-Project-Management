using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers
{
    public class AuthController : Controller
    {
        [Route("Auth")]
        public IActionResult Projects()
        {
            return View();
        }
    }
}
