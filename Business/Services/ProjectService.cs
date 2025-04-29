using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Extensions;
using Domain.Models;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;

    // CREATE
    public async Task<ProjectResult<ProjectModel>> AddProjectAsync(AddProjectForm form)
    {
        if (form == null)
            return ProjectResult<ProjectModel>.BadRequest("Form cannot be null.");

        var projectEntity = form.MapTo<ProjectEntity>();

        projectEntity.ProjectMembers = projectEntity.ProjectMembers ?? [];
        foreach (var projectMember in form.ProjectMembers)
        {
            projectEntity.ProjectMembers.Add(new ProjectMemberEntity
            {
                UserId = projectMember,
                ProjectId = projectEntity.Id
            });
        }

        try
        {
            await _projectRepository.AddAsync(projectEntity);
            var addProjectResult = await _projectRepository.SaveAsync();
            if (!addProjectResult.Success)
            {
                return ProjectResult<ProjectModel>.InternalServerErrror("Failed creating project.");
            }

            var createdProjectEntityWithIncludes = await _projectRepository.GetAsync(x => x.Id == projectEntity.Id, x => x.Client!, x => x.ProjectMembers!);
            var createdProjectWithInclues = createdProjectEntityWithIncludes.MapTo<ProjectModel>();

            return ProjectResult<ProjectModel>.Created(createdProjectWithInclues);
        }
        catch (Exception ex)
        {
            return ProjectResult<ProjectModel>.InternalServerErrror($"Unexpected error occured. {ex.Message}");
        }
    }
}
