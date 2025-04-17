namespace Domain.Models;

public class ProjectModel
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public string? ImageURI { get; set; }
    public string Name { get; set; } = null!;
    public ClientModel? Client { get; set; } = null!;
    public IEnumerable<ProjectMemberModel>? ProjectMembers { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; }
}
