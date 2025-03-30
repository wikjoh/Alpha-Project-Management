using Data.Entities;
using Domain.Models;

namespace Data.Interfaces;

public interface IUserProfileRepository : IBaseRepository<UserProfileEntity, UserProfileModel>
{
}
