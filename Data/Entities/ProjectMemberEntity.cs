using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class ProjectMemberEntity
{
    [Key] // Composite key set via fluent API
    public string UserId { get; set; } = null!;
    [Key] // Composite key set via fluent API
    public int ProjectId { get; set; }

    public MemberProfileEntity? MemberProfile { get; set; } = null!;
    public ProjectEntity? Project { get; set; } = null!;
}
