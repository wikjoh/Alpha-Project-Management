using Business.Interfaces;
using Business.Models;
using Business.Dtos;
using Data.Entities;
using Data.Interfaces;
using Domain.Extensions;
using Domain.Models;

namespace Business.Services;

public class UserProfileService(IUserProfileRepository userProfileRepository) : IUserProfileService
{
    private readonly IUserProfileRepository _userProfileRepository = userProfileRepository;


    // CREATE
    public async Task<UserProfileResult<UserProfileModel>> CreateUserProfileAsync(AddUserProfileForm form, string userId)
    {
        if (form == null)
            return UserProfileResult<UserProfileModel>.BadRequest("Form cannot be null.");

        var exists = (await _userProfileRepository.ExistsAsync(x => x.UserId == userId)).Success;
        if (exists)
            return UserProfileResult<UserProfileModel>.AlreadyExists($"User profile for UserId {userId} already exists.");

        var userProfileEntity = form.MapTo<UserProfileEntity>();
        userProfileEntity.UserId = userId;

        var createResult = await _userProfileRepository.AddAsync(userProfileEntity);
        var saveResult = await _userProfileRepository.SaveAsync();
        var createdUserProfile = (await _userProfileRepository.GetAsync(x => x.UserId == userProfileEntity.UserId)).Data;

        if (createdUserProfile != null)
            return UserProfileResult<UserProfileModel>.Created(createdUserProfile);

        if (createResult.Success && saveResult.Success && createdUserProfile == null)
            return UserProfileResult<UserProfileModel>.InternalServerErrror("Failed retrieving user profile after update.");

        return UserProfileResult<UserProfileModel>.InternalServerErrror("Failed creating user profile.");
    }


    // READ
    public async Task<UserProfileResult<IEnumerable<UserProfileModel>>> GetAllUserProfilesAsync()
    {
        var result = await _userProfileRepository.GetAllAsync();
        return result.MapTo<UserProfileResult<IEnumerable<UserProfileModel>>>();
    }

    public async Task<UserProfileResult<UserProfileModel>> GetUserProfileByIdAsync(string id)
    {
        var result = await _userProfileRepository.GetAsync(x => x.UserId == id);
        if (result.StatusCode == 404)
            return UserProfileResult<UserProfileModel>.NotFound($"UserProfile for UserId {id} not found.");

        return result.MapTo<UserProfileResult<UserProfileModel>>();
    }


    // UPDATE
    public async Task<UserProfileResult<UserProfileModel>> UpdateUserProfileAsync(EditUserProfileForm form)
    {
        if (form == null)
            return UserProfileResult<UserProfileModel>.BadRequest("Form cannot be null");

        var existingEntity = (await _userProfileRepository.GetEntityAsync(x => x.UserId == form.UserId)).Data;
        if (existingEntity == null)
            return UserProfileResult<UserProfileModel>.NotFound("UserId not found");

        existingEntity.ImageURI = form.ImageURI;
        existingEntity.FirstName = form.FirstName;
        existingEntity.LastName = form.LastName;
        existingEntity.JobTitle = form.JobTitle;
        existingEntity.Address = form.Address;
        existingEntity.DateOfBirth = form.DateOfBirth;

        var updateResult = _userProfileRepository.Update(existingEntity);
        var saveResult = await _userProfileRepository.SaveAsync();
        var updatedUserProfile = (await _userProfileRepository.GetAsync(x => x.UserId == form.UserId)).Data;

        if (updateResult.Success && saveResult.Success)
        {
            if (updatedUserProfile != null)
                return UserProfileResult<UserProfileModel>.Ok(updatedUserProfile);
            else
                return UserProfileResult<UserProfileModel>.InternalServerErrror("Failed retrieving user profile after update.");
        }
        else
            return UserProfileResult<UserProfileModel>.InternalServerErrror("Failed updating user profile.");
    }


    // DELETE
    public async Task<UserProfileResult<UserProfileModel>> DeleteUserProfileAsync(string id)
    {
        var exists = (await _userProfileRepository.ExistsAsync(x => x.UserId == id)).Success;
        if (!exists)
            return UserProfileResult<UserProfileModel>.NotFound($"User profile for user id {id} not found.");

        var entity = (await _userProfileRepository.GetEntityAsync(x => x.UserId == id)).Data;
        var result = _userProfileRepository.Delete(entity!);

        if (!result.Success)
            return UserProfileResult<UserProfileModel>.InternalServerErrror($"Failed deleting user profile for userid {id}");

        return UserProfileResult<UserProfileModel>.Ok();
    }
}
