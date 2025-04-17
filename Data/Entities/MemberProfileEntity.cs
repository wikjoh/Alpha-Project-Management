using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class MemberProfileEntity
{
    [Key]
    public string UserId { get; set; } = null!;
    public UserEntity? User { get; set; }

    [ProtectedPersonalData]
    [Column(TypeName = "varchar(200)")]
    public string? ImageURI { get; set; }

    [ProtectedPersonalData]
    [Column(TypeName = "varchar(200)")]
    public string? PhoneNumber { get; set; }

    [ProtectedPersonalData]
    [Column(TypeName = "nvarchar(40)")]
    public string JobTitle { get; set; } = null!;

    [ProtectedPersonalData]
    [Column(TypeName = "date")]
    public DateTime? DateOfBirth { get; set; }

    public MemberAddressEntity? MemberAddress { get; set; }
    public ICollection<ProjectMemberEntity>? ProjectMembers { get; set; }
}
