using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class UserProjectRepository(AppDbContext context) : BaseRepository<UserProjectEntity>(context), IUserProjectRepository
{
}
