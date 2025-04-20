namespace Domain.Models;

public class ClientModel
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public string? ImageURI { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;
    public ClientAddressModel ClientAddress { get; set; } = null!;
    public bool IsActive { get; set; }
}
