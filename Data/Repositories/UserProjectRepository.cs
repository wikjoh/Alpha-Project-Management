﻿using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories;

public class UserProjectRepository(AppDbContext context) : BaseRepository<UserProjectEntity, UserProjectModel>(context), IUserProjectRepository
{
}
