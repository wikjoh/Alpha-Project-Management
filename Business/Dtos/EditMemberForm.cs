namespace Business.Dtos;

public class EditMemberForm
{
    public string UserId { get; set; } = null!;
    public UserSignUpForm User { get; set; } = null!;
    public string? ImageURI { get; set; }
    public string? PhoneNumber { get; set; }
    public string JobTitle { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }

    public MemberAddressForm MemberAddress { get; set; } = null!;
}
