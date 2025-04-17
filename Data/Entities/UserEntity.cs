using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class UserEntity : IdentityUser
{
    public DateTime Created { get; set; } = DateTime.Now;

    [ProtectedPersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string FirstName { get; set; } = null!;

    [ProtectedPersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; } = null!;

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";

    public MemberProfileEntity? MemberProfile { get; set; }
}
