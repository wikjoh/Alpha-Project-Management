using Microsoft.AspNetCore.Http;

namespace Business.Dtos;

public class EditClientForm
{
    public int Id { get; set; }
    public IFormFile? ClientImage { get; set; }
    public string ClientName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public ClientAddressForm ClientAddress { get; set; } = null!;
}
