using Domain.Models;

namespace Business.Models;

public class UserResult : ServiceResult
{
    public IEnumerable<UserModel>? Data { get; set; }
}
