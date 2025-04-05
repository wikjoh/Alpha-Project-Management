using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class UserSignUpForm
{
    [Required(ErrorMessage = "Required")]
    [Display(Name = "Full Name", Prompt = "Your full name")]
    [DataType(DataType.Text)]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^[^@\s]+@[^\s@]+\.[^\s@]+$", ErrorMessage = "Invalid email")]
    [Display(Name = "Email address", Prompt = "Your email address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,20}$", ErrorMessage = "Invalid password")]
    [Display(Name = "Password", Prompt = "Enter your password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Confirm Password", Prompt = "Confirm your password")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Range(typeof(bool), "true", "true", ErrorMessage = "Required")]
    public bool TermsAndConditions { get; set; }
}
