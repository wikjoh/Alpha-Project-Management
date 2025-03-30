﻿using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories;

public class ClientRepository(AppDbContext context) : BaseRepository<ClientEntity, ClientModel>(context), IClientRepository
{
}
