using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class ProjectMemberEntity
{
    [Key]
    public string UserId { get; set; } = null!;
    [Key]
    public int ProjectId { get; set; }

    public MemberProfileEntity? MemberProfile { get; set; } = null!;
    public ProjectEntity? Project { get; set; } = null!;
}
