using Business.Dtos;
using Business.Models;
using Domain.Models;

namespace Business.Interfaces;
public interface IProjectService
{
    Task<ProjectResult<ProjectModel>> AddProjectAsync(AddProjectForm form);
    Task<ProjectResult<bool>> DeleteProjectAsync(int projectId);
    Task<ProjectResult<IEnumerable<ProjectModel>>> GetAllProjectsAsync();
    Task<ProjectResult<ProjectModel>> GetProjectByIdAsync(int id);
    Task<ProjectResult<ProjectModel>> UpdateProjectAsync(EditProjectForm form);
}