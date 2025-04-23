using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class MemberProfileModel
{
    public string UserId { get; set; } = null!;
    public UserModel User { get; set; } = null!;
    public string? ImageURI { get; set; }
    public string? PhoneNumber { get; set; }
    public string JobTitle { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }

    public MemberAddressModel? MemberAddress { get; set; }
    public IEnumerable<ProjectMemberModel>? ProjectMembers { get; set; }
}
