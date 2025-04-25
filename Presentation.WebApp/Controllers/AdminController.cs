using Business.Dtos;
using Business.Interfaces;
using Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models;
using System.Threading.Tasks;

namespace Presentation.WebApp.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin")]
public class AdminController(IClientService clientService, IMemberProfileService memberProfileService, IMemberService memberService) : Controller
{
    private readonly IClientService _clientService = clientService;
    private readonly IMemberProfileService _memberProfileService = memberProfileService;
    private readonly IMemberService _memberService = memberService;

    [Route("members")]
    public async Task<IActionResult> Members()
    {
        var result = await _memberProfileService.GetAllMemberProfilesAsync();
        var memberProfiles = result.Data;

        return View(memberProfiles);
    }

    [Route("addMember")]
    [HttpPost]
    public async Task<IActionResult> AddMember(AddMemberViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray());

            return BadRequest(new { success = false, errors });
        }

        var memberForm = vm.MapTo<AddMemberForm>();
        var result = await _memberService.AddMemberAsync(memberForm);

        return result.Success
           ? CreatedAtAction(nameof(AddMember), result.Data)
           : StatusCode(result.StatusCode, result.ErrorMessage);
    }


    [Route("clients")]
    public async Task<IActionResult> Clients()
    {
        var clientList = (await _clientService.GetAllClientsAsync()).Data;

        return View(clientList);
    }


    [Route("addClient")]
    [HttpPost]
    public async Task<IActionResult> AddClient(AddClientViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray());

            return BadRequest(new { success = false, errors });
        }

        var clientForm = vm.MapTo<AddClientForm>();
        var result = await _clientService.CreateClientAsync(clientForm);

        return result.Success
           ? CreatedAtAction(nameof(AddClient), result.Data)
           : StatusCode(result.StatusCode, result.ErrorMessage);
    }


    [Route("editClient")]
    [HttpPost]
    public async Task<IActionResult> EditClient(EditClientViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray());

            return BadRequest(new { success = false, errors });

        }

        var clientForm = vm.MapTo<EditClientForm>();
        var result = await _clientService.UpdateClientAsync(clientForm);

        return result.Success
           ? Ok(nameof(EditClient))
           : StatusCode(result.StatusCode, result.ErrorMessage);
    }


    [Route("getClient")]
    [HttpGet("getClient/id/{id}")]
    public async Task<IActionResult> GetClient(int id)
    {
        var result = await _clientService.GetClientByIdAsync(id);
        if (!result.Success || result.Data == null)
            return NotFound();

        var clientViewModel = result.Data.MapTo<EditClientViewModel>();

        return Ok(clientViewModel);
    }
}
