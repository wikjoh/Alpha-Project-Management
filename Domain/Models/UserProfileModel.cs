namespace Domain.Models;

public class UserProfileModel
{
    public string UserId { get; set; } = null!;
    public DateTime Created { get; set; }
    public string? ImageURI { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? JobTitle { get; set; }
    public string? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public bool? TermsAndConditions { get; set; }
}
