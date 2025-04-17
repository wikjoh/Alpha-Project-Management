namespace Domain.Models;

public class ClientAddressModel
{
    public int ClientId { get; set; }
    public ClientModel? Client { get; set; }
    public string StreetAddress { get; set; } = null!;
    public int PostalCode { get; set; }
    public string City { get; set; } = null!;
}
