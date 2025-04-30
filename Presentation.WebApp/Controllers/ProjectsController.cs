using Business.Dtos;
using Business.Interfaces;
using Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Project;
using Presentation.WebApp.Services.Interfaces;
using System.Threading.Tasks;

namespace Presentation.WebApp.Controllers;

[Authorize]
public class ProjectsController(IMemberService memberService, IClientService clientService, IProjectService projectService, IImageUploadService imageUploadService) : Controller
{
    private readonly IProjectService _projectService = projectService;
    private readonly IMemberService _memberService = memberService;
    private readonly IClientService _clientService = clientService;
    private readonly IImageUploadService _imageUploadService = imageUploadService;

    public async Task<IActionResult> Projects()
    {
        var projects = (await _projectService.GetAllProjectsAsync()).Data;

        return View(projects);
    }

    [Route("addProject")]
    [HttpPost]
    public async Task<IActionResult> AddProject(AddProjectViewModel vm)
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
            imagePath = await _imageUploadService.UploadImageAsync(vm.ProjectImage!, "projects");

            var projectForm = vm.MapTo<AddProjectForm>();
            projectForm.ClientId = vm.SelectedClientId;
            projectForm.ProjectMembers = vm.SelectedProjectMemberIds;
            projectForm.ImageURI = imagePath;

            var result = await _projectService.AddProjectAsync(projectForm);

            if (result.Success)
                return CreatedAtAction(nameof(AddProject), result.Data);

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

    [HttpGet("Projects/SearchClients/{searchTerm}")]
    public async Task<IActionResult> SearchClients(string searchTerm)
    {
        var result = await _clientService.GetActiveClientsIdNameBySearchTerm(searchTerm);
        if (!result.Success)
            return StatusCode(result.StatusCode, result.ErrorMessage);

        return Ok(result.Data);
    }

    [HttpGet("Projects/SearchMembers/{searchTerm}")]
    public async Task<IActionResult> SearchMembers(string searchTerm)
    {
        var result = await _memberService.GetMembersUseridNameBySearchTerm(searchTerm);
        if (!result.Success)
            return StatusCode(result.StatusCode, result.ErrorMessage);

        return Ok(result.Data);
    }
}
