using Data.Entities;
using Domain.Models;

namespace Data.Interfaces;

public interface IMemberProfileRepository : IBaseRepository<MemberProfileEntity, MemberProfileModel>
{
}
