namespace Domain.Models;

public class UserModel
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
}