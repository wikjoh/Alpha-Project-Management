using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models;

public class EditClientViewModel
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public string? ImageURI { get; set; }

    [Display(Name = "Client Image", Prompt = "Select an image")]
    [DataType(DataType.Upload)]
    public IFormFile? ClientImage { get; set; }

    [Display(Name = "Client Name", Prompt = "Enter client name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string Name { get; set; } = null!;

    [Display(Name = "Email", Prompt = "Enter email address")]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email")]
    public string Email { get; set; } = null!;

    [Display(Name = "PhoneNumber", Prompt = "Enter phone number")]
    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }

    public ClientAddressViewModel ClientAddress { get; set; } = null!;
}
