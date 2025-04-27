using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers;

[Authorize]
public class ProjectsController(IMemberService memberService, IClientService clientService) : Controller
{
    private readonly IMemberService _memberService = memberService;
    private readonly IClientService _clientService = clientService;

    public IActionResult Projects()
    {
        return View();
    }

    [HttpGet("SearchClients/{searchTerm}")]
    public async Task<IActionResult> SearchClients(string searchTerm)
    {
        var result = await _clientService.GetActiveClientsIdNameBySearchTerm(searchTerm);
        if (!result.Success)
            return StatusCode(result.StatusCode, result.ErrorMessage);

        return Ok(result.Data);
    }

    [HttpGet("SearchMembers/{searchTerm}")]
    public async Task<IActionResult> SearchMembers(string searchTerm)
    {
        var result = await _memberService.GetMembersUseridNameBySearchTerm(searchTerm);
        if (!result.Success)
            return StatusCode(result.StatusCode, result.ErrorMessage);

        return Ok(result.Data);
    }
}
