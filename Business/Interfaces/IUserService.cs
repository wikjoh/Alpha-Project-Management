using Business.Models;
using Business.Dtos;
using Domain.Models;

namespace Business.Interfaces;
public interface IUserService
{
    Task<UserResult<string?>> AddUserToRole(string userId, string role);
    Task<UserResult<UserModel>> CreateUserAsync(UserSignUpForm form, string password);
    Task<UserResult<UserModel>> CreateUserWithoutPasswordAsync(UserSignUpForm form);
}