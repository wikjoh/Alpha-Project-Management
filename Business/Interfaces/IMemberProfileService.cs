using Business.Dtos;
using Business.Models;
using Domain.Models;

namespace Business.Interfaces;
public interface IMemberProfileService
{
    Task<MemberProfileResult<MemberProfileModel>> CreateMemberProfileAsync(AddMemberForm form, string userId);
    Task<MemberProfileResult<bool?>> ExistsByIdAsync(string id);
}