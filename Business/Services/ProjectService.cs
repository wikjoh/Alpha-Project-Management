using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Extensions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

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

        foreach (var memberId in form.SelectedMemberIds)
        {
            projectEntity.ProjectMembers.Add(new ProjectMemberEntity
            {
                UserId = memberId,
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
            var createdProjectWithIncludes = createdProjectEntityWithIncludes.MapTo<ProjectModel>();

            return ProjectResult<ProjectModel>.Created(createdProjectWithIncludes);
        }
        catch (Exception ex)
        {
            return ProjectResult<ProjectModel>.InternalServerErrror($"Unexpected error occured. {ex.Message}");
        }
    }


    // READ
    public async Task<ProjectResult<IEnumerable<ProjectModel>>> GetAllProjectsAsync()
    {
        var repositoryResult = await _projectRepository.GetAllEntitiesByQueryAsync(query => query
            .Include(x => x.Client)
            .Include(x => x.ProjectMembers)
            .ThenInclude(x => x.MemberProfile)
            .OrderByDescending(x => x.Created));

        if (!repositoryResult.Success)
            return ProjectResult<IEnumerable<ProjectModel>>.InternalServerErrror("Failed retrieving project entities.");

        var entities = repositoryResult.Data ?? [];
        var projects = entities.Select(entity => entity.MapTo<ProjectModel>()).ToList();
        // Add specific mapping of ProjectMembers since MapTo only handles mapping one include level deep
        for (int i = 0; i < projects.Count; i++)
        {
            var project = projects[i];
            var entity = entities.ElementAt(i);
            project.ProjectMembers = entity.ProjectMembers.Select(pm => pm.MapTo<ProjectMemberModel>()).ToList();
        }

        return ProjectResult<IEnumerable<ProjectModel>>.Ok(projects);
    }

    public async Task<ProjectResult<ProjectModel>> GetProjectByIdAsync(int id)
    {
        var repositoryResult = await _projectRepository.GetEntityByQueryAsync(query => query
            .Where(x => x.Id == id)
            .Include(x => x.Client)
            .Include(x => x.ProjectMembers)
            .ThenInclude(x => x.MemberProfile)
            .ThenInclude(x => x.User)
            .OrderByDescending(x => x.Created));

        if (!repositoryResult.Success || repositoryResult.Data == null)
            return ProjectResult<ProjectModel>.InternalServerErrror("Failed retrieving entity.");

        var projectEntity = repositoryResult.Data;
        var project = projectEntity.MapTo<ProjectModel>();
        project.ProjectMembers = projectEntity.ProjectMembers.Select(pm => pm.MapTo<ProjectMemberModel>()).ToList();

        return ProjectResult<ProjectModel>.Ok(project);
    }


    // UPDATE
    public async Task<ProjectResult<ProjectModel>> UpdateProjectAsync(EditProjectForm form)
    {
        if (form == null)
            return ProjectResult<ProjectModel>.BadRequest("Form cannot be null.");

        var projectEntity = (await _projectRepository.GetEntityAsync(x => x.Id == form.Id, x => x.Client!, x => x.ProjectMembers)).Data;
        if (projectEntity == null)
            return ProjectResult<ProjectModel>.NotFound("Project not found.");

        projectEntity.ImageURI = form.ImageURI!;
        projectEntity.Name = form.Name;
        projectEntity.ClientId = form.ClientId;
        projectEntity.Description = form.Description;
        projectEntity.StartDate = form.StartDate;
        projectEntity.EndDate = form.EndDate;
        projectEntity.Budget = form.Budget;
        projectEntity.ProjectMembers = []; // Wipe previous projectmembers

        foreach (var memberId in form.SelectedMemberIds)
        {
            projectEntity.ProjectMembers.Add(new ProjectMemberEntity
            {
                UserId = memberId,
                ProjectId = projectEntity.Id
            });
        }

        _projectRepository.Update(projectEntity);
        var result = await _projectRepository.SaveAsync();
        if (!result.Success)
            return ProjectResult<ProjectModel>.InternalServerErrror("Failed updating project.");

        var updatedProjectEntity = (await _projectRepository.GetEntityByQueryAsync(query => query
            .Where(x => x.Id == form.Id)
            .Include(x => x.Client)
            .Include(x => x.ProjectMembers)
            .ThenInclude(x => x.MemberProfile)
            .OrderByDescending(x => x.Created))).Data;

        if (updatedProjectEntity == null)
            return ProjectResult<ProjectModel>.InternalServerErrror("Retrieved null entity after update.");

        var updatedProject = updatedProjectEntity.MapTo<ProjectModel>();
        updatedProject.ProjectMembers = updatedProjectEntity.ProjectMembers.Select(pm => pm.MapTo<ProjectMemberModel>()).ToList();

        return ProjectResult<ProjectModel>.Ok(updatedProject);
    }


    // DELETE
    public async Task<ProjectResult<bool>> DeleteProjectAsync(int projectId)
    {
        var projectEntity = (await _projectRepository.GetEntityAsync(x => x.Id == projectId)).Data;
        if (projectEntity == null)
            return ProjectResult<bool>.NotFound("Project not found.");

        _projectRepository.Delete(projectEntity);
        var result = await _projectRepository.SaveAsync();
        if (!result.Success)
            return ProjectResult<bool>.InternalServerErrror("Failed deleting project.");

        return ProjectResult<bool>.Ok(true);
    }
}
