using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Members()
        {
            return View();
        }
    }
}
