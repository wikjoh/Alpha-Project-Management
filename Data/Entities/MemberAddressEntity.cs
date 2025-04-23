using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class MemberAddressEntity
{
    [Key]
    public string UserId { get; set; } = null!;
    public MemberProfileEntity? MemberProfile { get; set; }

    [ProtectedPersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string? StreetAddress { get; set; }

    [ProtectedPersonalData]
    [Column(TypeName = "varchar(10)")]
    public string? PostalCode { get; set; }

    [ProtectedPersonalData]
    [Column(TypeName = "nvarchar(20)")]
    public string? City { get; set; }
}
