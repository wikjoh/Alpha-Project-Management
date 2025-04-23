using Business.Dtos;
using Business.Models;
using Domain.Models;

namespace Business.Interfaces;
public interface IMemberService
{
    Task<MemberProfileResult<MemberProfileModel>> AddMemberAsync(AddMemberForm form);
}