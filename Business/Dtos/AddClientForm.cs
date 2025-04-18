using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class AddClientForm
{
    public bool IsActive { get; set; }
    public string? ImageURI { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public ClientAddressForm ClientAddress { get; set; } = null!;
}
