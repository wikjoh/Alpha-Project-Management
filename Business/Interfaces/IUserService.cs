using Business.Models;
using Business.Dtos;
using Domain.Models;

namespace Business.Interfaces;
public interface IUserService
{
    Task<UserResult<UserModel>> CreateUserAsync(UserSignUpForm form);
}