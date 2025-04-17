using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectEntity
{
    [Key]
    public int Id { get; set; }
    public DateTime Created {  get; set; } = DateTime.Now;

    [Column(TypeName = "varchar(200)")]
    public string? ImageURI {  get; set; }

    [Column(TypeName = "nvarchar(200)")]
    public string Name { get; set; } = null!;

    public int ClientId { get; set; }
    public ClientEntity Client { get; set; } = null!;

    [Column(TypeName = "nvarchar(max)")]
    public string? Description { get; set; }

    [Column(TypeName = "date")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime? EndDate { get; set; }
    public decimal? Budget { get; set; }

    public ICollection<ProjectMemberEntity>? ProjectMembers { get; set; }
}
