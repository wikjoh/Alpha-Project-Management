﻿namespace Business.Dtos;

public class AddMemberForm
{
    public UserSignUpForm UserForm { get; set; } = null!;
    public string? ImageURI { get; set; }
    public string? PhoneNumber { get; set; }
    public string JobTitle { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }

    public MemberAddressForm MemberAddress { get; set; } = null!;
}
