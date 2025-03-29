using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class UserProjectEntity
{
    [Key]
    public string UserId { get; set; } = null!;
    [Key]
    public int ProjectId { get; set; }

    public UserEntity User { get; set; } = null!;
    public ProjectEntity Project { get; set; } = null!;
}
