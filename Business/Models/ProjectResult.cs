using Domain.Models;

namespace Business.Models;

public class ProjectResult : ServiceResult
{
    public IEnumerable<ProjectModel>? Data { get; set; }
}
