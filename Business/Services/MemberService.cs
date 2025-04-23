using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using Domain.Models;

namespace Business.Services;

public class MemberService(IMemberProfileRepository memberProfileRepository, IUserService userService, IMemberProfileService memberProfileService, IUserRepository userRepository) : IMemberService
{
    private readonly IMemberProfileRepository _memberProfileRepository = memberProfileRepository;
    private readonly IUserService _userService = userService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMemberProfileService _memberProfileService = memberProfileService;

    // CREATE
    // Attempts to create a new user, member profile & assign admin role.
    // If user already exists - create member profile & assign admin role.
    // If member profile already exists for given user - fail and do nothing.
    public async Task<MemberProfileResult<MemberProfileModel>> AddMemberAsync(AddMemberForm form)
    {
        if (form == null || form.UserForm == null)
            return MemberProfileResult<MemberProfileModel>.BadRequest("Form cannot be null.");

        try
        {
            await _userRepository.BeginTransactionAsync();
            var createUserResult = await _userService.CreateUserWithoutPasswordAsync(form.UserForm);

            switch (createUserResult.StatusCode)
            {
                case 201:
                    {
                        // User created successfully. Proceed with adding admin role and member profile
                        var userId = createUserResult.Data!.Id;
                        var addRoleResult = await _userService.AddUserToRoleAsync(userId, "Admin");
                        if (!addRoleResult.Success)
                        {
                            await _memberProfileRepository.RollbackTransactionAsync();
                            return MemberProfileResult<MemberProfileModel>.InternalServerErrror("Failed adding user to role.");
                        }

                        var addProfileResult = await _memberProfileService.CreateMemberProfileAsync(form, userId);
                        if (!addProfileResult.Success || addProfileResult.Data == null)
                        {
                            await _memberProfileRepository.RollbackTransactionAsync();
                            return MemberProfileResult<MemberProfileModel>.InternalServerErrror("Failed creating member profile.");
                        }

                        await _memberProfileRepository.CommitTransactionAsync();
                        var memberProfile = addProfileResult.Data;
                        return MemberProfileResult<MemberProfileModel>.Created(memberProfile);
                    }

                case 409:
                    {
                        // User already exists. Check if member profile already exists, if not then proceed with adding admin role and member profile
                        var userResult = await _userService.GetUserByEmailAsync(form.UserForm.Email);
                        if (!userResult.Success || userResult.Data == null)
                        {
                            await _memberProfileRepository.RollbackTransactionAsync();
                            return MemberProfileResult<MemberProfileModel>.InternalServerErrror("Failed retrieving existing user.");
                        }

                        var exists = await _memberProfileService.ExistsByIdAsync(userResult.Data.Id);
                        if (exists.StatusCode == 204)
                        {
                            await _memberProfileRepository.RollbackTransactionAsync();
                            return MemberProfileResult<MemberProfileModel>.AlreadyExists($"Member profile for user {form.UserForm.Email} already exists.");
                        }

                        var addRoleResult = await _userService.AddUserToRoleAsync(userResult.Data.Id, "Admin");
                        if (!addRoleResult.Success)
                        {
                            await _memberProfileRepository.RollbackTransactionAsync();
                            return MemberProfileResult<MemberProfileModel>.InternalServerErrror("Failed adding user to role.");
                        }

                        var userId = userResult.Data.Id;
                        var addProfileResult = await _memberProfileService.CreateMemberProfileAsync(form, userId);
                        if (!addProfileResult.Success || addProfileResult.Data == null)
                        {
                            await _memberProfileRepository.RollbackTransactionAsync();
                            return MemberProfileResult<MemberProfileModel>.InternalServerErrror("Failed creating member profile.");
                        }

                        await _memberProfileRepository.CommitTransactionAsync();
                        var memberProfile = addProfileResult.Data;
                        return MemberProfileResult<MemberProfileModel>.Created(memberProfile);
                    }

                default:
                    await _memberProfileRepository.RollbackTransactionAsync();
                    return MemberProfileResult<MemberProfileModel>.InternalServerErrror("Failed creating member profile.");
            }
        }
        catch (Exception ex)
        {
            await _memberProfileRepository.RollbackTransactionAsync();
            return MemberProfileResult<MemberProfileModel>.InternalServerErrror($"Unexpected error occured. {ex.Message}");
        }
    }
}
