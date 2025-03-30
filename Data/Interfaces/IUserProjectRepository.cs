using Data.Entities;
using Domain.Models;

namespace Data.Interfaces;

public interface IUserProjectRepository : IBaseRepository<UserProjectEntity, UserProjectModel>
{
}
