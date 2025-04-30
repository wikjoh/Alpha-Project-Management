using Business.Dtos;
using Business.Models;
using Domain.Models;

namespace Business.Interfaces;
public interface IProjectService
{
    Task<ProjectResult<ProjectModel>> AddProjectAsync(AddProjectForm form);
    Task<ProjectResult<IEnumerable<ProjectModel>>> GetAllProjectsAsync();
}