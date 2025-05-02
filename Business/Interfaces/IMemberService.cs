using Business.Dtos;
using Business.Dtos.API;
using Business.Models;
using Domain.Models;

namespace Business.Interfaces;
public interface IMemberService
{
    Task<MemberProfileResult<MemberProfileModel>> AddMemberAsync(AddMemberForm form);
    Task<MemberProfileResult<MemberProfileModel>> GetMemberByIdAsync(string id);
    Task<MemberProfileResult<IEnumerable<MemberUseridNameImg>>> GetMembersUseridNameImgBySearchTerm(string searchTerm);
    Task<MemberProfileResult<MemberProfileModel>> UpdateMemberAsync(EditMemberForm form);
}