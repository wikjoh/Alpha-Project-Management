using Business.Dtos.API;
using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Project;

public class EditProjectViewModel
{
    public int Id { get; set; }
    public string? CurrentImageURI { get; set; }

    [Display(Name = "Project Image", Prompt = "Select an image")]
    [DataType(DataType.Upload)]
    public IFormFile? ProjectImage { get; set; }

    public string? ImageURI { get; set; }

    [Display(Name = "Project Name", Prompt = "Project Name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string Name { get; set; } = null!;

    [Required]
    public int SelectedClientId { get; set; }
    public ClientIdNameImg? Client { get; set; }

    [Display(Name = "Description")]
    [DataType(DataType.Text)]
    public string? Description { get; set; }

    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    public DateTime? EndDate { get; set; }

    public List<string> SelectedMemberIds { get; set; } = [];
    public IEnumerable<ProjectMemberProfileNavOnly>? ProjectMembers { get; set; }

    public decimal? Budget { get; set; }
}
