using Business.Dtos;
using Business.Interfaces;
using Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models;
using System.Threading.Tasks;

namespace Presentation.WebApp.Controllers;

//[Authorize]
[Route("admin")]
public class AdminController(IClientService clientService) : Controller
{
    private readonly IClientService _clientService = clientService;

    [Route("members")]
    public IActionResult Members()
    {
        return View();
    }

    [Route("clients")]
    public async Task<IActionResult> Clients()
    {
        var clientList = (await _clientService.GetAllClients()).Data;

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
        clientForm.ClientAddress = vm.ClientAddress.MapTo<ClientAddressForm>();
        var result = await _clientService.CreateClientAsync(clientForm);

        return result.Success
           ? CreatedAtAction(nameof(AddClient), result.Data)
           : StatusCode(result.StatusCode, result.ErrorMessage);
    }

    [Route("editClient")]
    [HttpPost]
    public IActionResult EditClient(EditClientForm form)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray());

            return BadRequest(new { success = false, errors });

        }

        // send data to service

        return Ok(new { success = true });
    }
}
