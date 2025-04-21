using Business.Models;
using Business.Dtos;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Extensions;
using Business.Interfaces;

namespace Business.Services;

public class UserService(UserManager<UserEntity> userManager) : IUserService
{
    private readonly UserManager<UserEntity> _userManager = userManager;


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

        var result = await _userManager.CreateAsync(userEntity, password);

        if (result.Succeeded)
        {
            var createdUserEntity = _userManager.Users.FirstOrDefault(x => x.Id == userEntity.Id);
            if (createdUserEntity == null)
                return UserResult<UserModel>.InternalServerErrror("Failed retrieving user entity after creation");

            var createdUser = createdUserEntity.MapTo<UserModel>();
            return UserResult<UserModel>.Created(createdUser);
        }

        else return UserResult<UserModel>.InternalServerErrror("Failed creating user.");
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

        var result = await _userManager.CreateAsync(userEntity);

        if (result.Succeeded)
        {
            var createdUserEntity = _userManager.Users.FirstOrDefault(x => x.Id == userEntity.Id);
            if (createdUserEntity == null)
                return UserResult<UserModel>.InternalServerErrror("Failed retrieving user entity after creation");

            var createdUser = createdUserEntity.MapTo<UserModel>();
            return UserResult<UserModel>.Created(createdUser);
        }

        else return UserResult<UserModel>.InternalServerErrror("Failed creating user.");
    }

}
