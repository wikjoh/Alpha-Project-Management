using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class ClientRepository(AppDbContext context) : BaseRepository<ClientEntity>(context), IClientRepository
{
}
