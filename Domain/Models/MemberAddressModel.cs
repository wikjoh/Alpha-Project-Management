namespace Domain.Models;

public class MemberAddressModel
{
    public string UserId { get; set; } = null!;
    public MemberProfileModel? MemberProfile { get; set; }
    public string? StreetAddress { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
}
