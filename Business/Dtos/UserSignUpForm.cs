using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class UserSignUpForm
{

    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool TermsAndConditions { get; set; }
}
