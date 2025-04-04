using Business.Models;
using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces;
public interface IUserService
{
    Task<UserResult<UserModel>> CreateAsync(UserSignUpForm form);
}