using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Domain.Extensions;
using Domain.Models;

namespace Business.Services;

public class ClientService(IClientRepository clientRepository, IClientAddressService clientAddressService) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IClientAddressService _clientAddressService = clientAddressService;

    // CREATE
    public async Task<ClientResult<ClientModel>> CreateClientAsync(AddClientForm form)
    {
        if (form == null)
            return ClientResult<ClientModel>.BadRequest("Form cannot be null.");

        var exists = ((await _clientRepository.ExistsAsync(x => x.Email == form.Email)).Success || (await _clientRepository.ExistsAsync(x => x.Name == form.Name)).Success);
        if (exists)
            return ClientResult<ClientModel>.AlreadyExists($"Client with email {form.Email} or name {form.Name} already exists.");

        var clientEntity = form.MapTo<ClientEntity>();
        var clientAddressForm = form.ClientAddress.MapTo<ClientAddressForm>();


        await _clientRepository.BeginTransactionAsync();

        try
        {
            await _clientRepository.AddAsync(clientEntity);
            var createClientResult = await _clientRepository.SaveAsync();
            if (!createClientResult.Success)
            {
                await _clientRepository.RollbackTransactionAsync();
                return ClientResult<ClientModel>.InternalServerErrror($"Failed creating client entity for client {form.Name}.");
            }

            var createAddressResult = await _clientAddressService.CreateClientAddressAsync(clientAddressForm, clientEntity.Id);
            if (!createAddressResult.Success)
            {
                await _clientRepository.RollbackTransactionAsync();
                return ClientResult<ClientModel>.InternalServerErrror($"Failed creating client address entity for client {form.Name}.");
            }

            var createdClientEntityWithAddress = await _clientRepository.GetAsync(x => x.Id == clientEntity.Id, x => x.ClientAddress);
            var createdClientWithAddress = createdClientEntityWithAddress.MapTo<ClientModel>();

            await _clientRepository.CommitTransactionAsync();
            return ClientResult<ClientModel>.Created(createdClientWithAddress);
        }
        catch (Exception ex)
        {
            await _clientRepository.RollbackTransactionAsync();
            return ClientResult<ClientModel>.InternalServerErrror($"Unexpected error occurred. {ex.Message}");
        }
    }
}
