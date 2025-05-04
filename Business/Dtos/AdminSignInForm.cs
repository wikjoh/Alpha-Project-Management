using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class AdminSignInForm
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; }
}
