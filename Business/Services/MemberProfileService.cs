using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Extensions;
using Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services;

public class MemberProfileService(IMemberProfileRepository memberProfileRepository) : IMemberProfileService
{
    private readonly IMemberProfileRepository _memberProfileRepository = memberProfileRepository;

    // CREATE
    public async Task<MemberProfileResult<MemberProfileModel>> CreateMemberProfileAsync(AddMemberForm form, string userId)
    {
        if (form == null)
            return MemberProfileResult<MemberProfileModel>.BadRequest("Form cannot be null.");

        try
        {
            var entity = form.MapTo<MemberProfileEntity>();
            entity.UserId = userId;

            await _memberProfileRepository.AddAsync(entity);
            var createResult = await _memberProfileRepository.SaveAsync();
            if (!createResult.Success)
                return MemberProfileResult<MemberProfileModel>.InternalServerErrror("Failed creating member profile.");

            var getResult = await _memberProfileRepository.GetAsync(x => x.UserId == userId);
            if (!getResult.Success || getResult.Data == null)
                return MemberProfileResult<MemberProfileModel>.InternalServerErrror("Failed retrieving member profile after creation.");

            var memberProfile = getResult.Data;
            return MemberProfileResult<MemberProfileModel>.Created(memberProfile);
        }
        catch (Exception ex)
        {
            return MemberProfileResult<MemberProfileModel>.InternalServerErrror($"Unexpected error occured. {ex.Message}");
        }
    }


    // READ
    public async Task<MemberProfileResult<bool?>> ExistsByIdAsync(string id)
    {
        if (id.Trim().IsNullOrEmpty())
            return MemberProfileResult<bool?>.BadRequest("Id cannot be empty.");

        var result = await _memberProfileRepository.ExistsAsync(x => x.UserId == id);
        return result.MapTo<MemberProfileResult<bool?>>();
    }

    public async Task<MemberProfileResult<IEnumerable<MemberProfileModel>>> GetAllMemberProfilesAsync()
    {
        var result = await _memberProfileRepository.GetAllAsync(false, null, null, x => x.User, x => x.MemberAddress!);
        if (!result.Success)
            return MemberProfileResult<IEnumerable<MemberProfileModel>>.InternalServerErrror("Failed retrieving member profiles.");

        var memberProfiles = result.Data;
        return memberProfiles != null
            ? MemberProfileResult<IEnumerable<MemberProfileModel>>.Ok(memberProfiles)
            : MemberProfileResult<IEnumerable<MemberProfileModel>>.NoContent();
    }
}
