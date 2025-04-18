using Business.Dtos;
using Business.Models;
using Domain.Models;

namespace Business.Interfaces;

public interface IClientAddressService
{
    Task<ClientAddressResult<ClientAddressModel>> CreateClientAddressAsync(ClientAddressForm form, int? clientId);
}