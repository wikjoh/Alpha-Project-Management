using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Domain.Dtos;
using Microsoft.EntityFrameworkCore;
using Domain.Extensions;

namespace Business.Services;

public class UserService(IUserRepository userRepository, UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager)
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;


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

        var result = await _userManager.CreateAsync(userEntity, form.Password);
        if (result.Succeeded)
        {
            var createdUser = (await _userManager.FindByIdAsync(userEntity.Id))!.MapTo<UserModel>();
            return UserResult<UserModel>.Created(createdUser);
        }
        else
        {
            return UserResult<UserModel>.InternalServerErrror("Failed creating user.");
        }
    }
}
