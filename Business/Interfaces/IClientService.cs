using Business.Dtos;
using Business.Dtos.API;
using Business.Models;
using Domain.Models;

namespace Business.Interfaces;
public interface IClientService
{
    Task<ClientResult<ClientModel>> CreateClientAsync(AddClientForm form);
    Task<ClientResult<IEnumerable<ClientIdName>>> GetActiveClientsIdNameBySearchTerm(string searchTerm);
    Task<ClientResult<IEnumerable<ClientModel>>> GetAllClientsAsync();
    Task<ClientResult<ClientModel>> GetClientByIdAsync(int id);
    Task<ClientResult<ClientModel>> UpdateClientAsync(EditClientForm form);
}