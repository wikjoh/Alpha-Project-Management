using Business.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers;

[Route("admin")]
public class AdminController : Controller
{
    [Route("members")]
    public IActionResult Members()
    {
        return View();
    }

    [Route("clients")]
    public IActionResult Clients()
    {
        return View();
    }

    [HttpPost]
    public IActionResult AddClient(AddClientForm form)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Clients");

        return View();
    }


    [HttpPost]
    public IActionResult EditClient(AddClientForm form)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Clients");

        return View();
    }
}
