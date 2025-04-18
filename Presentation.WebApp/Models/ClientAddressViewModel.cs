using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models;

public class ClientAddressViewModel
{
    [Display(Name = "Street Address", Prompt = "Enter street address")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string StreetAddress { get; set; } = null!;

    [Display(Name = "Postal Code", Prompt = "Enter postal code")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public int PostalCode { get; set; }

    [Display(Name = "City", Prompt = "Enter city")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string City { get; set; } = null!;

    [Display(Name = "Country", Prompt = "Enter country")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string Country { get; set; } = null!;
}
