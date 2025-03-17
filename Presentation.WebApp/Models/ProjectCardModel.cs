namespace Presentation.WebApp.Models;

public class ProjectCardModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Company { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string DueTime { get; set; } = null!;

}
