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
}
