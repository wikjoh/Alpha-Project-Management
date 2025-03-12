using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers
{
    public class ProjectsController : Controller
    {
        //[Route("Projects")]
        public IActionResult Projects()
        {
            return View();
        }
    }
}
