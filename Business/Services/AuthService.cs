using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Business.Dtos;
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

    public async Task<AuthResult<string?>> LogoutAsync()
    {
        try
        {
            await _signInManager.SignOutAsync();
            return AuthResult<string?>.Ok();
        }
        catch (Exception ex)
        {
            return AuthResult<string?>.InternalServerErrror(ex.Message);
        }
    }

    public async Task<AuthResult<UserModel>> SignUpAsync(UserSignUpForm form, string password)
    {
        var result = await _userService.CreateUserAsync(form, password);
        return result.MapTo<AuthResult<UserModel>>();
    }
}
