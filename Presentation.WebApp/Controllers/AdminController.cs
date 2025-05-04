using Business.Dtos;
using Business.Interfaces;
using Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Client;
using Presentation.WebApp.Models.Member;
using Presentation.WebApp.Services.Interfaces;
using System.Threading.Tasks;

namespace Presentation.WebApp.Controllers;

[Authorize(Roles = "Admin")]
[Route("admin")]
public class AdminController(IClientService clientService, IMemberProfileService memberProfileService, IMemberService memberService, IImageUploadService imageUploadService) : Controller
{
    private readonly IClientService _clientService = clientService;
    private readonly IMemberProfileService _memberProfileService = memberProfileService;
    private readonly IMemberService _memberService = memberService;
    private readonly IImageUploadService _imageUploadService = imageUploadService;

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

        string? imagePath = null;

        // Wrap in trycatch in order to delete image in case something unexpected occurs
        try
        {
            imagePath = await _imageUploadService.UploadImageAsync(vm.MemberImage!, "members");

            var memberForm = vm.MapTo<AddMemberForm>();
            memberForm.ImageURI = imagePath != null ? imagePath : "/images/memberDefaultAvatar.svg"; // set default image if none chosen

            var result = await _memberService.AddMemberAsync(memberForm);
            if (result.Success)
                return CreatedAtAction(nameof(AddMember), result.Data);

            if (imagePath != null)
                _imageUploadService.DeleteImage(imagePath);
            return Problem("Failed handling submit.");
        }
        catch (Exception)
        {
            if (imagePath != null)
                _imageUploadService.DeleteImage(imagePath);
            return Problem("Failed handling submit.");
        }
    }

    [Route("editMember")]
    [HttpPost]
    public async Task<IActionResult> EditMember(EditMemberViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray());

            return BadRequest(new { success = false, errors });
        }

        string? imagePath = null;
        string? currentImage = (await _memberService.GetMemberByIdAsync(vm.UserId)).Data?.ImageURI;

        // Wrap in trycatch in order to delete image in case something unexpected occurs
        try
        {
            var memberForm = vm.MapTo<EditMemberForm>();
            memberForm.ImageURI = currentImage;

            if (vm.MemberImage != null)
            {
                imagePath = await _imageUploadService.UpdateImageAsync(vm.MemberImage!, "projects", currentImage!);
                memberForm.ImageURI = imagePath;
            }

            var result = await _memberService.UpdateMemberAsync(memberForm);
            if (result.Success)
                return Ok(nameof(EditMember));

            if (imagePath != null)
                _imageUploadService.DeleteImage(imagePath);
            return Problem("Failed handling submit.");
        }
        catch (Exception)
        {
            if (imagePath != null)
                _imageUploadService.DeleteImage(imagePath);
            return Problem("Failed handling submit.");
        }
    }

    [HttpGet("getMember/id/{id}")]
    public async Task<IActionResult> GetMember(string id)
    {
        var result = await _memberService.GetMemberByIdAsync(id);
        if (!result.Success || result.Data == null)
            return NotFound();

        var memberViewModel = result.Data.MapTo<EditMemberViewModel>();

        return Ok(memberViewModel);
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

        string? imagePath = null;

        // Wrap in trycatch in order to delete image in case something unexpected occurs
        try
        {
            imagePath = await _imageUploadService.UploadImageAsync(vm.ClientImage!, "clients");

            var clientForm = vm.MapTo<AddClientForm>();
            clientForm.ImageURI = imagePath ?? "/images/clientDefaultAvatar.svg"; // set default image if none chosen

            var result = await _clientService.CreateClientAsync(clientForm);
            if (result.Success)
                return CreatedAtAction(nameof(AddClient), result.Data);

            if (imagePath != null)
                _imageUploadService.DeleteImage(imagePath);
            return Problem("Failed handling submit.");
        }
        catch (Exception)
        {
            if (imagePath != null)
                _imageUploadService.DeleteImage(imagePath);
            return Problem("Failed handling submit.");
        }
        
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

        string? imagePath = null;
        string? currentImage = (await _clientService.GetClientByIdAsync(vm.Id)).Data?.ImageURI;

        // Wrap in trycatch in order to delete image in case something unexpected occurs
        try
        {
            var clientForm = vm.MapTo<EditClientForm>();
            clientForm.ImageURI = currentImage;

            if (vm.ClientImage != null)
            {
                imagePath = await _imageUploadService.UpdateImageAsync(vm.ClientImage!, "projects", currentImage!);
                clientForm.ImageURI = imagePath;
            }

            var result = await _clientService.UpdateClientAsync(clientForm);
            if (result.Success)
                return Ok(nameof(EditClient));

            if (imagePath != null)
                _imageUploadService.DeleteImage(imagePath);
            return Problem("Failed handling submit.");
        }
        catch (Exception)
        {
            if (imagePath != null)
                _imageUploadService.DeleteImage(imagePath);
            return Problem("Failed handling submit.");
        }
    }

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
