using Business.Dtos;
using Business.Models;
using Domain.Models;

namespace Business.Interfaces;
public interface IClientService
{
    Task<ClientResult<ClientModel>> CreateClientAsync(AddClientForm form);
}