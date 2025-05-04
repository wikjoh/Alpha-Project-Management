using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Member;

public class EditMemberViewModel
{
    public string UserId { get; set; } = null!;
    public MemberUserViewModel User { get; set; } = new();

    [Display(Name = "Member Image", Prompt = "Select an image")]
    [DataType(DataType.Upload)]
    public IFormFile? MemberImage { get; set; }

    public string? ImageURI { get; set; }

    [Display(Name = "PhoneNumber", Prompt = "Enter phone number")]
    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Job Title", Prompt = "Enter job title")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string JobTitle { get; set; } = null!;

    public MemberAddressViewModel MemberAddress { get; set; } = new();

    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth", Prompt = "Enter date of birth")]
    public DateTime? DateOfBirth { get; set; }
}
