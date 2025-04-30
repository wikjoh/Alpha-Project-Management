using Business.Dtos;
using Business.Interfaces;
using Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Project;
using System.Threading.Tasks;

namespace Presentation.WebApp.Controllers;

[Authorize]
public class ProjectsController(IMemberService memberService, IClientService clientService, IProjectService projectService) : Controller
{
    private readonly IProjectService _projectService = projectService;
    private readonly IMemberService _memberService = memberService;
    private readonly IClientService _clientService = clientService;

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

        var projectForm = vm.MapTo<AddProjectForm>();
        projectForm.ClientId = vm.SelectedClientId;
        projectForm.ProjectMembers = vm.SelectedProjectMemberIds;

        var result = await _projectService.AddProjectAsync(projectForm);

        return result.Success
           ? CreatedAtAction(nameof(AddProject), result.Data)
           : StatusCode(result.StatusCode, result.ErrorMessage);
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
