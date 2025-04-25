using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models;

public class MemberAddressViewModel
{
    [Display(Name = "Street Address", Prompt = "Enter street address")]
    [DataType(DataType.Text)]
    public string? StreetAddress { get; set; }

    [Display(Name = "Postal Code", Prompt = "Enter postal code")]
    [DataType(DataType.Text)]
    public string? PostalCode { get; set; }

    [Display(Name = "City", Prompt = "Enter city")]
    [DataType(DataType.Text)]
    public string? City { get; set; }
}
