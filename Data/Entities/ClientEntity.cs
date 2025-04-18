using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Index(nameof(Name), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class ClientEntity
{
    [Key]
    public int Id { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public bool IsActive { get; set; } = true;

    [Column(TypeName = "varchar(200)")]
    public string? ImageURI { get; set; }

    [Column(TypeName = "nvarchar(200)")]
    public string Name { get; set; } = null!;

    [Column(TypeName = "varchar(256)")]
    public string Email { get; set; } = null!;

    [Column(TypeName = "varchar(20)")]
    public string PhoneNumber { get; set; } = null!;

    public ICollection<ProjectEntity>? Projects { get; set; }
    public ClientAddressEntity ClientAddress { get; set; } = null!;
}
