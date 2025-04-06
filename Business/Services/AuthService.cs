using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Domain.Dtos;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public class AuthService(SignInManager<UserEntity> signInManager, IUserService userService) : IAuthService
{
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly IUserService _userService = userService;

    public async Task<AuthResult<string>> LoginAsync(UserSignInForm form)
    {
        if (form == null)
            return AuthResult<string>.BadRequest("Login form cannot be null.");

        var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, false, false);
        if (result.Succeeded)
            return AuthResult<string>.Ok("User successfully authenticated.");

        return AuthResult<string>.Unauthorized("User authentication failed.");
    }

    public async Task<AuthResult<UserModel>> SignUpAsync(UserSignUpForm form)
    {
        var result = await _userService.CreateAsync(form);
        return result.MapTo<AuthResult<UserModel>>();
    }
}
