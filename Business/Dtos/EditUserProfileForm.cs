using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class EditUserProfileForm
{
    [Required]
    public string UserId { get; set; } = null!;

    [DataType(DataType.Upload)]
    public string? ImageURI { get; set; }

    [Required(ErrorMessage = "Required")]
    [Display(Name = "First Name", Prompt = "Your first name")]
    [DataType(DataType.Text)]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Last Name", Prompt = "Your last name")]
    [DataType(DataType.Text)]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^[^@\s]+[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email")]
    [Display(Name = "Email address", Prompt = "Your email address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [RegularExpression(@"^\d{5,20}$", ErrorMessage = "Phone number must be at least 5 digits and at most 20 digits")]
    [Display(Name = "Phone", Prompt = "Your phone number")]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; } = null!;

    [Display(Name = "Job Title", Prompt = "Your job title")]
    [DataType(DataType.Text)]
    public string? JobTitle { get; set; }

    [Display(Name = "Address", Prompt = "Your address")]
    [DataType(DataType.Text)]
    public string? Address { get; set; }

    [Display(Name = "Date of Birth")]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }
}
