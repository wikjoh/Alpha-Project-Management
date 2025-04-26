using Business.Dtos;
using Business.Models;
using Domain.Models;

namespace Business.Interfaces;
public interface IMemberService
{
    Task<MemberProfileResult<MemberProfileModel>> AddMemberAsync(AddMemberForm form);
    Task<MemberProfileResult<MemberProfileModel>> GetMemberByIdAsync(string id);
    Task<MemberProfileResult<MemberProfileModel>> UpdateMemberAsync(EditMemberForm form);
}