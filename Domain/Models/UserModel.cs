namespace Domain.Models;

public class UserModel
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime Created { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string FullName { get; set; } = null!;

    public MemberProfileModel? MemberProfile { get; set; }
}