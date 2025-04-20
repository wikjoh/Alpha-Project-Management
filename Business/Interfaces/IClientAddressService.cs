using Business.Dtos;
using Business.Models;
using Data.Interfaces;
using Domain.Models;

namespace Business.Interfaces;

public interface IClientAddressService
{
    Task<ClientAddressResult<ClientAddressModel>> CreateClientAddressAsync(ClientAddressForm form, int? clientId);
    Task<ClientAddressResult<ClientAddressModel>> GetClientAddressAsync(int? id);
}