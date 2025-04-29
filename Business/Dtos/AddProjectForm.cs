namespace Business.Dtos;

public class AddProjectForm
{
    public DateTime Created { get; set; } = DateTime.Now;
    public string? ImageURI { get; set; }
    public string Name { get; set; } = null!;
    public int ClientId { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public IEnumerable<string> ProjectMembers { get; set; } = null!;
    public decimal? Budget { get; set; }
}
