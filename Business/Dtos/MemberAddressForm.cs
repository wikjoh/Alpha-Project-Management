namespace Business.Dtos;

public class MemberAddressForm
{
    public string StreetAddress { get; set; } = null!;
    public int PostalCode { get; set; }
    public string City { get; set; } = null!;
}
