using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Domain.Extensions;
using Domain.Models;
using System.Diagnostics;

namespace Business.Services;

public class ClientAddressService(IClientAddressRepository clientAddressRepository) : IClientAddressService
{
    private readonly IClientAddressRepository _clientAddressRepository = clientAddressRepository;

    // CREATE
    public async Task<ClientAddressResult<ClientAddressModel>> CreateClientAddressAsync(ClientAddressForm form, int? clientId)
    {
        if (form == null || clientId == null)
            return ClientAddressResult<ClientAddressModel>.BadRequest("Parameters cannot be null.");

        var exists = (await _clientAddressRepository.ExistsAsync(x => x.ClientId == (int)clientId));
        if (exists.StatusCode == 204)
        {
            Console.WriteLine($"Address for client with id {clientId} already exists.");
            return ClientAddressResult<ClientAddressModel>.AlreadyExists($"Address for client with id {clientId} already exists.");
        }

        var clientAddressEntity = form.MapTo<ClientAddressEntity>();
        clientAddressEntity.ClientId = (int)clientId;

        try
        {
            await _clientAddressRepository.AddAsync(clientAddressEntity);
            var result = await _clientAddressRepository.SaveAsync();

            if (!result.Success)
                return ClientAddressResult<ClientAddressModel>.InternalServerErrror($"Failed creating address for client id {clientId}.");

            var createdClientAddressEntity = await _clientAddressRepository.GetAsync(x => x.ClientId == clientId);
            if (createdClientAddressEntity == null)
                return ClientAddressResult<ClientAddressModel>.InternalServerErrror($"Failed retrieving entity for client id {clientId} after creation.");

            var createdClientAddress = createdClientAddressEntity.MapTo<ClientAddressModel>();
            return ClientAddressResult<ClientAddressModel>.Created(createdClientAddress);
        }
        catch (Exception ex)
        {
            return ClientAddressResult<ClientAddressModel>.InternalServerErrror($"Unexpected error occurred. {ex.Message}");
        }
    }

    // READ
    public async Task<ClientAddressResult<ClientAddressModel>> GetClientAddressAsync(int? id)
    {
        if (id == null)
            return ClientAddressResult<ClientAddressModel>.BadRequest("Id cannot be null");

        var result = await _clientAddressRepository.GetAsync(x => x.ClientId == id);
        var resultMapped = result.MapTo<ClientAddressResult<ClientAddressModel>>();
        return resultMapped;
    }
}
