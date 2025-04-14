using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class UserSignInForm
{
    [Required(ErrorMessage = "Required")]
    [Display(Name = "Email address", Prompt = "Your email address")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Required")]
    [Display(Name = "Password", Prompt = "Enter your password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

}
