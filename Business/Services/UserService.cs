using Business.Models;
using Business.Dtos;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Extensions;
using Business.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services;

public class UserService(IUserRepository userRepository, UserManager<UserEntity> userManager, RoleManager<IdentityRole> roleManager) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;


    // CREATE
    public async Task<UserResult<UserModel>> CreateUserAsync(UserSignUpForm form, string password)
    {
        if (form == null)
            return UserResult<UserModel>.BadRequest("Form cannot be null.");

        if (await _userManager.Users.AnyAsync(u => u.Email == form.Email))
            return UserResult<UserModel>.AlreadyExists("User with given email address already exists.");

        var userEntity = form.MapTo<UserEntity>();
        userEntity.UserName = form.Email;
        userEntity.Email = form.Email;

        try
        {
            await _userRepository.BeginTransactionAsync();

            var createUserResult = await _userManager.CreateAsync(userEntity, password);
            if (!createUserResult.Succeeded)
            {
                await _userRepository.RollbackTransactionAsync();
                return UserResult<UserModel>.InternalServerErrror("Failed creating user.");
            }

            var createdUserEntity = _userManager.Users.FirstOrDefault(x => x.Id == userEntity.Id);
            if (createdUserEntity == null)
            {
                await _userRepository.RollbackTransactionAsync();
                return UserResult<UserModel>.InternalServerErrror("Failed retrieving user entity after creation.");
            }

            var addToRoleResult = await AddUserToRoleAsync(userEntity.Id, "User");
            if (!addToRoleResult.Success)
            {
                await _userRepository.RollbackTransactionAsync();
                return UserResult<UserModel>.InternalServerErrror("Failed adding role to user after user creation.");
            }

            await _userRepository.CommitTransactionAsync();
            var createdUser = createdUserEntity.MapTo<UserModel>();
            return UserResult<UserModel>.Created(createdUser);
        }
        catch (Exception ex)
        {
            await _userRepository.RollbackTransactionAsync();
            return UserResult<UserModel>.InternalServerErrror($"Unexpected error occured. {ex.Message}");
        }
    }

    public async Task<UserResult<UserModel>> CreateUserWithoutPasswordAsync(UserSignUpForm form)
    {
        if (form == null)
            return UserResult<UserModel>.BadRequest("Form cannot be null.");

        if (await _userManager.Users.AnyAsync(u => u.Email == form.Email))
            return UserResult<UserModel>.AlreadyExists("User with given email address already exists.");

        var userEntity = form.MapTo<UserEntity>();
        userEntity.UserName = form.Email;
        userEntity.Email = form.Email;

        try
        {
            await _userRepository.BeginTransactionAsync();

            var createUserResult = await _userManager.CreateAsync(userEntity);
            if (!createUserResult.Succeeded)
            {
                await _userRepository.RollbackTransactionAsync();
                return UserResult<UserModel>.InternalServerErrror("Failed creating user.");
            }

            var createdUserEntity = _userManager.Users.FirstOrDefault(x => x.Id == userEntity.Id);
            if (createdUserEntity == null)
            {
                await _userRepository.RollbackTransactionAsync();
                return UserResult<UserModel>.InternalServerErrror("Failed retrieving user entity after creation.");
            }

            var addToRoleResult = await AddUserToRoleAsync(userEntity.Id, "User");
            if (!addToRoleResult.Success)
            {
                await _userRepository.RollbackTransactionAsync();
                return UserResult<UserModel>.InternalServerErrror("Failed adding role to user after user creation.");
            }

            await _userRepository.CommitTransactionAsync();
            var createdUser = createdUserEntity.MapTo<UserModel>();
            return UserResult<UserModel>.Created(createdUser);
        }
        catch (Exception ex)
        {
            await _userRepository.RollbackTransactionAsync();
            return UserResult<UserModel>.InternalServerErrror($"Unexpected error occured. {ex.Message}");
        }
    }

    public async Task<UserResult<string?>> AddUserToRoleAsync(string userId, string role)
    {
        if (!await _roleManager.RoleExistsAsync(role))
            return UserResult<string?>.NotFound($"Role {role} does not exist.");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return UserResult<string?>.NotFound($"No user with id {userId} found.");

        var result = await _userManager.AddToRoleAsync(user, role);
        return result.Succeeded
            ? UserResult<string?>.Ok($"UserId {userId} added to role {role} successfully.")
            : UserResult<string?>.InternalServerErrror($"Failed adding userId {userId} to role {role}.");
    }


    // READ
    public async Task<UserResult<UserModel>> GetUserByEmailAsync(string email)
    {
        if (email.Trim().IsNullOrEmpty())
            return UserResult<UserModel>.BadRequest("Email parameter cannot be empty.");

        var userEntity = await _userManager.FindByEmailAsync(email);
        if (userEntity == null)
            return UserResult<UserModel>.NotFound($"User with email {email} not found.");

        var user = userEntity.MapTo<UserModel>();
        return UserResult<UserModel>.Ok(user);
    }

    public async Task<UserResult<bool>> IsUserAdminAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return UserResult<bool>.NotFound($"User with email {email} not found.");

        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

        return UserResult<bool>.Ok(isAdmin);
    }
}
