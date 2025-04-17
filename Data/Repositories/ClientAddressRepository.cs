using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories;

public class ClientAddressRepository(AppDbContext context) : BaseRepository<ClientAddressEntity, ClientAddressModel>(context), IClientAddressRepository
{
}
