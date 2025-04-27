using Business.Dtos;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
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
        //var clientAddressForm = form.ClientAddress.MapTo<ClientAddressForm>();


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

            // ClientAddresEntity created automatically through navigation prop relation, no need to create manually
            //var createAddressResult = await _clientAddressService.CreateClientAddressAsync(clientAddressForm, clientEntity.Id);
            //if (!createAddressResult.Success)
            //{
            //    await _clientRepository.RollbackTransactionAsync();
            //    return ClientResult<ClientModel>.InternalServerErrror($"Failed creating client address entity for client {form.Name}.");
            //}

            await _clientRepository.CommitTransactionAsync();

            var createdClientEntityWithAddress = await _clientRepository.GetAsync(x => x.Id == clientEntity.Id, x => x.ClientAddress);
            var createdClientWithAddress = createdClientEntityWithAddress.MapTo<ClientModel>();

            return ClientResult<ClientModel>.Created(createdClientWithAddress);
        }
        catch (Exception ex)
        {
            await _clientRepository.RollbackTransactionAsync();
            return ClientResult<ClientModel>.InternalServerErrror($"Unexpected error occurred. {ex.Message}");
        }
    }


    // READ
    public async Task<ClientResult<IEnumerable<ClientModel>>> GetAllClientsAsync()
    {
        var repositoryResult = await _clientRepository.GetAllAsync(false, x => x.Created, includes: x => x.ClientAddress);
        if (!repositoryResult.Success)
            return ClientResult<IEnumerable<ClientModel>>.InternalServerErrror("Failed retrieving clients.");

        var serviceResult = repositoryResult.MapTo<ClientResult<IEnumerable<ClientModel>>>();
        return serviceResult;
    }

    public async Task<ClientResult<ClientModel>> GetClientByIdAsync(int id)
    {
        var repositoryResult = await _clientRepository.GetAsync(x => x.Id == id, includes: x => x.ClientAddress);
        if (!repositoryResult.Success || repositoryResult.Data == null)
            return ClientResult<ClientModel>.NotFound($"Client with Id {id} not found.");

        var clientModel = repositoryResult.Data.MapTo<ClientModel>();
        return ClientResult<ClientModel>.Ok(clientModel);
    }


    // UPDATE
    public async Task<ClientResult<ClientModel>> UpdateClientAsync(EditClientForm form)
    {
        if (form == null)
            return ClientResult<ClientModel>.BadRequest("Form cannot be null.");

        var client = (await _clientRepository.GetEntityAsync(x => x.Id == form.Id, includes: x => x.ClientAddress)).Data;
        if (client == null)
            return ClientResult<ClientModel>.NotFound("Client not found.");

        client.IsActive = form.IsActive;
        client.ImageURI = form.ImageURI;
        client.Name = form.Name;
        client.Email = form.Email;
        client.PhoneNumber = form.PhoneNumber!;
        client.ClientAddress = form.ClientAddress.MapTo<ClientAddressEntity>();

        _clientRepository.Update(client);
        var result = await _clientRepository.SaveAsync();
        if (!result.Success)
            return ClientResult<ClientModel>.InternalServerErrror("Failed updating client.");

        var updatedClient = (await _clientRepository.GetAsync(x => x.Id == form.Id, includes: x => x.ClientAddress)).Data;
        return updatedClient != null
            ? ClientResult<ClientModel>.Ok(updatedClient)
            : ClientResult<ClientModel>.InternalServerErrror("Failed retrieving client after update.");
    }
}
