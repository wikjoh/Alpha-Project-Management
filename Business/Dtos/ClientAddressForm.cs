namespace Business.Dtos;

public class ClientAddressForm
{
    public string StreetAddress { get; set; } = null!;
    public int PostalCode { get; set; }
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
}
