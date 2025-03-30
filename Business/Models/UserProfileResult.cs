using Domain.Models;

namespace Business.Models;

public class UserProfileResult : ServiceResult
{
    public IEnumerable<UserProfileModel>? Data { get; set; }
}
