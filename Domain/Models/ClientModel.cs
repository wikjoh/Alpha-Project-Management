namespace Domain.Models;

public class ClientModel
{
    public string Id { get; set; } = null!;
    public DateTime Created { get; set; }
    public string? ImageURI { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
}
