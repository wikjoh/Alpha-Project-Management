using Business.Models;
using Business.Dtos;
using Domain.Models;

namespace Business.Interfaces;
public interface IAuthService
{
    Task<AuthResult<string>> LoginAsync(UserSignInForm form);
    Task<AuthResult<string?>> LogoutAsync();
    Task<AuthResult<UserModel>> SignUpAsync(UserSignUpForm form);
}