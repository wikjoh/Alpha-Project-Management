using Domain.Models;

namespace Business.Models;

public class UserProjectResult : ServiceResult
{
    public IEnumerable<UserProjectModel>? Data { get; set; }
}