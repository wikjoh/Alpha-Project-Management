﻿using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Data.Repositories;

public class UserProfileRepository(AppDbContext context) : BaseRepository<UserProfileEntity, UserProfileModel>(context), IUserProfileRepository
{
}
