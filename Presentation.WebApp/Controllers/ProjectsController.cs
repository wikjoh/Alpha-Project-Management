using Business.Dtos;
using Business.Dtos.API;
using Business.Interfaces;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Client;
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
            projectForm.ImageURI = imagePath != null ? imagePath : "/images/projectDefaultAvatar.svg"; // set default image if none chosen

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

    [Route("editProject")]
    [HttpPost]
    public async Task<IActionResult> EditProject(EditProjectViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray());

            return BadRequest(new { success = false, errors });

        }

        var projectForm = vm.MapTo<EditProjectForm>();
        var result = await _projectService.UpdateProjectAsync(projectForm);

        return result.Success
           ? Ok(nameof(EditProject))
           : Problem("Failed handling submit.");
    }

    [HttpGet("getProject/id/{id}")]
    public async Task<IActionResult> GetProject(int id)
    {
        var result = await _projectService.GetProjectByIdAsync(id);
        if (!result.Success || result.Data == null)
            return NotFound();

        var project = result.Data;

        var projectViewModel = project.MapTo<EditProjectViewModel>();
        //projectViewModel.SelectedClientId = project.ClientId;
        if (project.ProjectMembers != null)
        {
            //projectViewModel.SelectedProjectMemberIds.AddRange(project.ProjectMembers.Select(pm => pm.UserId));
            //projectViewModel.ProjectMembers = project.ProjectMembers.Select(pm => pm.MapTo<ProjectMemberProfileNavOnly>()).ToList();
            projectViewModel.ProjectMembers = project.ProjectMembers.Select(pm =>
            {
                var simplified = pm.MapTo<ProjectMemberProfileNavOnly>();
                simplified.MemberProfile!.FullName = pm.MemberProfile!.User.FullName;
                return simplified;
            });
        }

        return Ok(projectViewModel);
    }

    [HttpGet("Projects/SearchClients/{searchTerm}")]
    public async Task<IActionResult> SearchClients(string searchTerm)
    {
        var result = await _clientService.GetActiveClientsIdNameImgBySearchTerm(searchTerm);
        if (!result.Success)
            return Problem("Failed retrieving clients.");

        return Ok(result.Data);
    }

    [HttpGet("Projects/SearchMembers/{searchTerm}")]
    public async Task<IActionResult> SearchMembers(string searchTerm)
    {
        var result = await _memberService.GetMembersUseridNameImgBySearchTerm(searchTerm);
        if (!result.Success)
            return Problem("Failed retrieving members.");

        return Ok(result.Data);
    }
}
