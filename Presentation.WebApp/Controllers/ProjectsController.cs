using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers;

//[Authorize]
[Route("projects")]
public class ProjectsController : Controller
{
    [Route("projects")]
    public IActionResult Projects()
    {
        return View();
    }
}
