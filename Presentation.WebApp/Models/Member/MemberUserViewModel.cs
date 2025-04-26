using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Member;

public class MemberUserViewModel
{
    [Required(ErrorMessage = "Required")]
    [Display(Name = "First Name", Prompt = "Your first name")]
    [DataType(DataType.Text)]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Last Name", Prompt = "Enter last name")]
    [DataType(DataType.Text)]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^[^@\s]+@[^\s@]+\.[^\s@]+$", ErrorMessage = "Invalid email")]
    [Display(Name = "Email address", Prompt = "Enter email address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
}
