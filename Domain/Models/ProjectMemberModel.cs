namespace Domain.Models;

public class ProjectMemberModel
{
    public string UserId { get; set; } = null!;
    public int ProjectId { get; set; }

    public MemberProfileModel? MemberProfile { get; set; }
    public ProjectModel? Project { get; set; }
}