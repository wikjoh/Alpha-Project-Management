using Domain.Models;
using System.ComponentModel;

namespace Presentation.WebApp.Models.Project;

public class ProjectCardViewModel
{

    public int Id { get; set; }
    public string? ImageURI { get; set; }
    public string Name { get; set; } = null!;
    public ClientModel Client { get; set; } = null!;
    public IEnumerable<ProjectMemberModel> ProjectMembers { get; set; } = [];
    public string? Description { get; set; }
    public DateTime? EndDate { get; set; }


    public (string? value, string? unit) DueTime()
    {
        if (EndDate == null)
            return ("Unscheduled", null);

        if (EndDate < DateTime.Now)
            return ("Completed", null);

        int daysRemaining = ((DateTime)EndDate - DateTime.Today).Days;
        if (daysRemaining > 7)
            return ((daysRemaining % 7).ToString(), daysRemaining % 7 > 1 ? "weeks left" : "week left");
        else
            return (daysRemaining.ToString(), daysRemaining > 1 ? "days left" : "day left");
    }
}