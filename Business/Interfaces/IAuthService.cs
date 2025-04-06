using Business.Models;
using Domain.Dtos;

namespace Business.Interfaces;
public interface IAuthService
{
    Task<AuthResult<string>> LoginAsync(UserSignInForm form);
}