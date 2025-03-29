using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class UserRepository(AppDbContext context) : BaseRepository<UserEntity>(context), IUserRepository
{
}
