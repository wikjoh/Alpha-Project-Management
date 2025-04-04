using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using Domain.Extensions;
using Business.Interfaces;

namespace Business.Services;

public class UserService(IUserRepository userRepository, UserManager<UserEntity> userManager, IUserProfileService userProfileService) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUserProfileService _userProfileService = userProfileService;
    private readonly UserManager<UserEntity> _userManager = userManager;


    // CREATE
    public async Task<UserResult<UserModel>> CreateAsync(UserSignUpForm form)
    {
        if (form == null)
            return UserResult<UserModel>.BadRequest("Form cannot be null.");

        if (await _userManager.Users.AnyAsync(u => u.Email == form.Email))
            return UserResult<UserModel>.AlreadyExists("User with given email address already exists.");

        var userEntity = new UserEntity
        {
            UserName = form.Email,
            Email = form.Email,
        };

        await _userRepository.BeginTransactionAsync();

        var createUserResult = await _userManager.CreateAsync(userEntity, form.Password);
        if (createUserResult.Succeeded)
        {
            var createUserProfileResult = await _userProfileService.CreateUserProfileAsync(form.MapTo<AddUserProfileForm>(), userEntity.Id);
            if (createUserProfileResult.Success)
            {
                await _userRepository.CommitTransactionAsync();
                var createdUserEntity = _userManager.Users.Include(u => u.UserProfile).FirstOrDefault(x => x.Id == userEntity.Id);
                var createdUser = createdUserEntity!.MapTo<UserModel>();
                return UserResult<UserModel>.Created(createdUser);
            }
            else
            {
                await _userRepository.RollbackTransactionAsync();
                return UserResult<UserModel>.InternalServerErrror("Failed creating user profile.");
            }
        }
        else
        {
            await _userRepository.RollbackTransactionAsync();
            return UserResult<UserModel>.InternalServerErrror("Failed creating user.");
        }
    }

}
