namespace Business.Dtos.API;

public class MemberProfileSimplified
{
    public string UserId { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? ImageURI { get; set; }
}
