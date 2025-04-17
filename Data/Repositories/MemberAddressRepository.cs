using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories;

public class MemberAddressRepository(AppDbContext context) : BaseRepository<MemberAddressEntity, MemberAddressModel>(context), IMemberAddressRepository
{
}
