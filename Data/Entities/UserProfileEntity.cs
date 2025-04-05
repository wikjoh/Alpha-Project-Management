using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class UserProfileEntity
{
    [Key]
    public string UserId { get; set; } = null!;
    public UserEntity User { get; set; } = null!;

    public DateTime Created { get; set; } = DateTime.Now;

    [Column(TypeName = "varchar(200)")]
    public string? ImageURI { get; set; }

    [Column(TypeName = "nvarchar(201)")]
    public string FullName { get; set; } = null!;

    [Column(TypeName = "nvarchar(100)")]
    public string? FirstName { get; set; }

    [Column(TypeName = "nvarchar(100)")]
    public string? LastName { get; set; }

    [Column(TypeName = "varchar(20)")]
    public string? PhoneNumber { get; set; }

    [Column(TypeName = "nvarchar(40)")]
    public string? JobTitle { get; set; }

    [Column(TypeName = "nvarchar(200)")]
    public string? Address { get; set; }

    [Column(TypeName = "date")]
    public DateTime? DateOfBirth { get; set; }

    public bool TermsAndConditions { get; set; }


    public void UpdateFullName()
    {
        if (string.IsNullOrWhiteSpace(FullName) && (!string.IsNullOrWhiteSpace(FirstName) || !string.IsNullOrWhiteSpace(LastName)))
        {
            FullName = $"{FirstName} {LastName}".Trim();
        }
    }
}