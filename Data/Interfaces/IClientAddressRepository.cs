using Data.Entities;
using Domain.Models;

namespace Data.Interfaces;

public interface IClientAddressRepository : IBaseRepository<ClientAddressEntity, ClientAddressModel>
{
}
