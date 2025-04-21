using Microsoft.AspNetCore.Http;

namespace Business.Dtos;

public class EditClientForm
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public string? ImageURI { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public ClientAddressForm ClientAddress { get; set; } = null!;
}
