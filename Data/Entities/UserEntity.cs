using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

public class UserEntity : IdentityUser
{
    public UserProfileEntity UserProfile { get; set; } = null!;
    public ICollection<UserProjectEntity> UserProjects { get; set; } = [];
}
