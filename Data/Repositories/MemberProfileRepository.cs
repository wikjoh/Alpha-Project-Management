using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories;

public class MemberProfileRepository(AppDbContext context) : BaseRepository<MemberProfileEntity, MemberProfileModel>(context), IMemberProfileRepository
{
}
