using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class UserProfileEntity
{
    [Key]
    public string UserId { get; set; } = null!;
    public UserEntity User { get; set; } = null!;

    public DateTime Created { get; set; } = DateTime.Now;

    [ProtectedPersonalData]
    [Column(TypeName = "varchar(200)")]
    public string? ImageURI { get; set; }

    [ProtectedPersonalData]
    [Column(TypeName = "nvarchar(201)")]
    public string FullName { get; set; } = null!;

    [ProtectedPersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string? FirstName { get; set; }

    [ProtectedPersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string? LastName { get; set; }

    [ProtectedPersonalData]
    [Column(TypeName = "varchar(20)")]
    public string? PhoneNumber { get; set; }

    [ProtectedPersonalData]
    [Column(TypeName = "nvarchar(40)")]
    public string? JobTitle { get; set; }

    [ProtectedPersonalData]
    [Column(TypeName = "nvarchar(200)")]
    public string? Address { get; set; }

    [ProtectedPersonalData]
    [Column(TypeName = "date")]
    public DateTime? DateOfBirth { get; set; }

    public bool TermsAndConditions { get; set; }
}